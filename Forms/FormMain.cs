using PottingLabelPrinter.Forms;
using PottingLabelPrinter.Models;
using PottingLabelPrinter.Printer;
using PottingLabelPrinter.Services;
using PottingLabelPrinter.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PottingLabelPrinter
{
    public partial class FormMain : Form
    {
        private readonly PottingCompletionDetector _pottingDetector = new PottingCompletionDetector();
        private readonly Color _lightBorderColor = SystemColors.ControlDark;
        private const int DefaultDpi = 203;

        // Modbus (2단계 최소 OOP)
        private readonly ModbusSession _modbusSession = new ModbusSession();
        private ModbusPollingService? _polling;

        private readonly PottingDoneSpec _doneSpec = new PottingDoneSpec
        {
            Slaves = new byte[] { 1 },
            Function = PottingLabelPrinter.Modbus.FunctionCode.ReadDiscreteInputs,
            Address = 0,
            BitIndex = 0,
            Invert = false,
            IntervalMs = 200
        };

        private const int BoardBaudRate = 9600;

        // Count
        private int _totalCount = 0;
        private int _okCount = 0;
        private int _ngCount = 0;

        // Tray Barcode (날짜별 1부터, 화면은 마지막 출력 라벨 표시)
        private DateTime _traySeqDate = DateTime.MinValue;
        private int _traySeq = 1;

        private readonly Dictionary<int, string> _trayResults = new Dictionary<int, string>();
        private int _maxTraySeq = 0;

        // Grid 헤더 클릭으로 전체 선택 토글할 때 쓰는 상태값
        private bool _selectAllChecked = false;

        /// <summary>
        /// 상태 확인 테스트용
        /// </summary>
        private FormDoneProbe? _doneProbeForm;

        // ====== (추가) 종료까지 유지되는 “연결 감시/자동 재연결” 타이머 ======
        private readonly System.Windows.Forms.Timer _reconnectTimer = new System.Windows.Forms.Timer();
        private int _healthFailStreak = 0;
        private const int ReconnectTickMs = 1000;
        private const int FailThresholdToReconnect = 3;

        private readonly TimeSpan _autoPrintMinInterval = TimeSpan.FromMilliseconds(300);
        private DateTime _lastAutoPrintTriggeredAt = DateTime.MinValue;

        /// <summary>
        /// IN2 상태일 때만 txtTrayBarcode backgroundcolor = white
        /// </summary>
        private readonly System.Windows.Forms.Timer _uiStateTimer = new System.Windows.Forms.Timer();

        // (변경) 블링크가 아니라 통신 상태 색상 표시용
        private readonly UiCommStatusIndicator _commStatusIndicator;

        public FormMain()
        {
            InitializeComponent();

            // (변경) 텍스트 ForeColor 상태 표시용 (점멸 없음)
            _commStatusIndicator = new UiCommStatusIndicator(lblTrayBarcodePrint, 120);

            EnsureDefaultSavePath();

            ApplyGridStyle();
            ApplyTestDefaults();

            btnComSetting.Click += BtnComSetting_Click;
            btnPathSetting.Click += BtnPathSetting_Click;
            btnPrintSetting.Click += BtnPrintSetting_Click;

            btnPrint.Click += BtnPrint_Click;
            btnReset.Click += BtnReset_Click;

            Load += FormMain_Load;
            FormClosed += FormMain_FormClosed;

            InitTraySequence();
            WireGridSelectHandlers();

            // 테스트용
            //btnDoneProbe.Click += btnDoneProbe_Click;
            //btnDoneProbe.Visible = false;
        }

        private void FormMain_Load(object? sender, EventArgs e)
        {
            // 1) 저장된 포트로 "종료까지" 연결 유지 (가능하면 즉시 Connect)
            TryConnectBoardFromSettings();

            // 2) Polling 서비스 준비 + 종료까지 유지
            _polling = new ModbusPollingService(_modbusSession, _doneSpec);
            _polling.PottingDoneDetected += Polling_PottingDoneDetected;
            _polling.PollingTxSucceeded += Polling_TxSucceeded;
            _polling.PollingErrorDetected += Polling_ErrorDetected;

            // 연결 여부와 관계 없이 Start는 켜둬도 됨 (Tick에서 IsOpen 체크로 안전하게 skip)
            _polling.Start();

            // 3) 오늘 로그 복원 (복원도 최신이 위로 보이도록 처리)
            RestoreTodayStateIfExists();

            // 4) UI 상태 타이머 (IN2에 따라 txtTrayBarcode 배경 표시) - 종료까지 유지
            _uiStateTimer.Interval = 100;
            _uiStateTimer.Tick += UiStateTimer_Tick;
            _uiStateTimer.Start();

            // 5) (추가) 종료까지 유지되는 자동 재연결/헬스체크 타이머
            _reconnectTimer.Interval = ReconnectTickMs;
            _reconnectTimer.Tick += ReconnectTimer_Tick;
            _reconnectTimer.Start();

            HookRunStartNoUi();
        }

        // =========================
        // (추가) 재연결/헬스체크: 종료까지 반복
        // =========================
        private void ReconnectTimer_Tick(object? sender, EventArgs e)
        {
            var port = (Properties.Settings.Default.ComBoardPort ?? "").Trim();
            if (string.IsNullOrWhiteSpace(port))
                return;

            if (!_modbusSession.IsOpen)
            {
                TryReconnect(port, "IsOpen=false");
                return;
            }

            // 헬스체크: DiscreteInputs 2bit 읽기
            byte slave = (_doneSpec.Slaves != null && _doneSpec.Slaves.Length > 0) ? _doneSpec.Slaves[0] : (byte)1;
            ushort addr = _doneSpec.Address;
            ushort count = 2;

            if (_modbusSession.TryReadDiscreteInputs(slave, addr, count, out _, out _))
            {
                _healthFailStreak = 0;
                return;
            }

            _healthFailStreak++;
            if (_healthFailStreak >= FailThresholdToReconnect)
            {
                TryReconnect(port, $"HealthFailStreak={_healthFailStreak}");
            }
        }

        private void TryReconnect(string portName, string reason)
        {
            _healthFailStreak = 0;

            try { _modbusSession.Disconnect(); } catch { }

            try
            {
                _modbusSession.Connect(portName, BoardBaudRate);

                // 재연결 직후 오탐 방지
                if (_polling != null)
                    _polling.ResetLatch();
            }
            catch
            {
                // 재연결 실패도 앱은 계속 동작하게 둔다. 다음 Tick에서 재시도
            }
        }

        // =========================
        // UI 상태 타이머: IN2 상태면 흰색
        // =========================
        private void UiStateTimer_Tick(object? sender, EventArgs e)
        {
            if (_polling == null)
                return;

            if (_polling.LastIn2)
                txtTrayBarcode.BackColor = Color.White;
            else
                txtTrayBarcode.BackColor = SystemColors.Control;
        }

        // =========================
        // 테스트용 Probe
        // =========================
        private void btnDoneProbe_Click(object sender, EventArgs e)
        {
            if (_doneProbeForm != null && !_doneProbeForm.IsDisposed)
            {
                _doneProbeForm.Focus();
                return;
            }

            _doneProbeForm = new FormDoneProbe(
                readDone: () => _polling?.LastIn2 ?? false,
                readRawInputsByte: () => _polling?.LastInputsByte ?? (byte)0,
                intervalMs: 100,
                titleSuffix: "IN2 유지시간 측정"
            );

            _doneProbeForm.Show(this);
        }

        private void BtnPrintSetting_Click(object sender, EventArgs e)
        {
            using (var form = new FormPrintSetting())
            {
                form.ShowDialog(this);
            }
        }

        // =========================
        // 종료: 프로그램 종료 시에만 정리
        // =========================
        private void FormMain_FormClosed(object? sender, FormClosedEventArgs e)
        {
            try
            {
                _uiStateTimer.Stop();
                _uiStateTimer.Tick -= UiStateTimer_Tick;
            }
            catch { }

            try
            {
                _reconnectTimer.Stop();
                _reconnectTimer.Tick -= ReconnectTimer_Tick;
            }
            catch { }

            if (_polling != null)
            {
                _polling.PottingDoneDetected -= Polling_PottingDoneDetected;
                _polling.PollingTxSucceeded -= Polling_TxSucceeded;
                _polling.PollingErrorDetected -= Polling_ErrorDetected;
                _polling.Dispose();
                _polling = null;
            }

            _commStatusIndicator.Dispose();
            _modbusSession.Dispose();
        }

        // =========================
        // 초기 연결: 저장된 포트로 Connect (가능하면)
        // =========================
        private void TryConnectBoardFromSettings()
        {
            var port = (Properties.Settings.Default.ComBoardPort ?? "").Trim();
            if (string.IsNullOrWhiteSpace(port))
                return;

            try
            {
                if (!_modbusSession.IsOpen)
                    _modbusSession.Connect(port, BoardBaudRate);
            }
            catch
            {
                // 연결 실패는 앱을 죽이지 않고 둔다.
            }
        }

        // =========================
        // 폴링 이벤트: 완료 감지 -> 자동 출력
        // =========================
        private void Polling_PottingDoneDetected(object? sender, PottingDoneDetectedEventArgs e)
        {
            // (변경) 초록색 표시 (점멸 없음)
            _commStatusIndicator.SetDone();

            var now = DateTime.Now;
            if (now - _lastAutoPrintTriggeredAt < _autoPrintMinInterval)
                return;

            _lastAutoPrintTriggeredAt = now;
            PrintSingleAuto();
        }

        private void Polling_TxSucceeded(object? sender, EventArgs e)
        {
            // (변경) 파란색 표시 (점멸 없음)
            _commStatusIndicator.SetTx();
        }

        private void Polling_ErrorDetected(object? sender, ModbusCommErrorEventArgs e)
        {
            // (변경) 빨간색 표시 (점멸 없음)
            _commStatusIndicator.SetError();
        }

        // =========================
        // Setting 버튼: 포트 변경 시 예외적으로 재연결
        // =========================
        private void BtnComSetting_Click(object? sender, EventArgs e)
        {
            using var frm = new FormPortSetting();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);

            TryReconnectBoardKeepPolling();
        }

        /// <summary>
        /// 기본은 종료까지 Open 유지.
        /// 단, 사용자가 Setting에서 포트를 바꾼 경우는 예외적으로 재연결.
        /// Polling은 종료까지 계속 지(세션만 재연결).
        /// </summary>
        private void TryReconnectBoardKeepPolling()
        {
            try
            {
                var port = (Properties.Settings.Default.ComBoardPort ?? "").Trim();
                if (string.IsNullOrWhiteSpace(port))
                    return;

                _modbusSession.Disconnect();
                _modbusSession.Connect(port, BoardBaudRate);

                if (_polling != null)
                    _polling.ResetLatch();
            }
            catch
            {
                // 실패해도 앱은 계속 동작
            }
        }

        private void HookRunStartNoUi()
        {
            // lblTotal이 이미 있다면
            lblTotal.Click += (_, __) =>
            {
                int current = AutoLabelSequenceState.GetCurrentNo();
                using (var f = new FormRunStartNo(current))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        AutoLabelSequenceState.SetCurrentNo(f.SelectedNo);

                        // 선택: 라벨 텍스트에 반영
                        // lblTotal.Text = $"Total: ... (NextNo={AutoLabelSequenceState.GetCurrentNo()})";
                    }
                }
            };
        }

        private void BtnPathSetting_Click(object? sender, EventArgs e)
        {
            using var frm = new FormPathSetting();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void EnsureDefaultSavePath()
        {
            var exeDir = Application.StartupPath;
            var defaultDir = Path.Combine(exeDir, "Data");

            var saved = (Properties.Settings.Default.SavePath ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(saved))
                return;

            Properties.Settings.Default.SavePath = defaultDir;
            Properties.Settings.Default.Save();

            if (!Directory.Exists(defaultDir))
                Directory.CreateDirectory(defaultDir);
        }

        private void InitTraySequence()
        {
            _traySeqDate = GetProductionDate(DateTime.Now);
            _traySeq = 1;

            try { txtTrayBarcode.ReadOnly = true; } catch { }

            txtTrayBarcode.Text = "-";
        }

        private void EnsureTraySeqForToday()
        {
            var productionDate = GetProductionDate(DateTime.Now);
            if (_traySeqDate.Date != productionDate.Date)
                ResetProductionDayState(productionDate, clearGrid: true);
        }

        // =========================
        // 통신 상태 표시: 라벨 텍스트 ForeColor만 변경
        // =========================
        private sealed class UiCommStatusIndicator : IDisposable
        {
            private readonly Label _label;
            private readonly Color _originalForeColor;

            private readonly int _minIntervalMs;
            private DateTime _lastSetAt = DateTime.MinValue;
            private Color _lastColor = Color.Empty;

            public UiCommStatusIndicator(Label label, int minIntervalMs)
            {
                _label = label ?? throw new ArgumentNullException(nameof(label));
                _originalForeColor = _label.ForeColor;
                _minIntervalMs = Math.Max(50, minIntervalMs);
            }

            public void SetTx()
            {
                Set(Color.DodgerBlue);
            }

            public void SetDone()
            {
                Set(Color.LimeGreen);
            }

            public void SetError()
            {
                Set(Color.OrangeRed);
            }

            private void Set(Color color)
            {
                var now = DateTime.Now;
                if (color == _lastColor && (now - _lastSetAt).TotalMilliseconds < _minIntervalMs)
                    return;

                _lastSetAt = now;
                _lastColor = color;

                _label.ForeColor = color;
            }

            public void ResetToDefault()
            {
                _label.ForeColor = _originalForeColor;
                _lastColor = Color.Empty;
                _lastSetAt = DateTime.MinValue;
            }

            public void Dispose()
            {
                // Timer/리소스 없음
            }
        }

        private string BuildTrayBarcodeText(DateTime now, int traySeq)
        {
            return $"TRAY{traySeq:0000} {now:yyyy-MM-dd HH:mm:ss}";
        }

        private void AdvanceTraySeqAfterOkPrint()
        {
            EnsureTraySeqForToday();
            _traySeq++;
        }

        private void WireGridSelectHandlers()
        {
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridView1_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != 0)
                return;

            _selectAllChecked = !_selectAllChecked;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                row.Cells[0].Value = _selectAllChecked;
            }
        }

        private bool IsRowChecked(DataGridViewRow row)
        {
            try
            {
                var v = row.Cells[0].Value;
                if (v == null) return false;
                if (v is bool b) return b;
                return bool.TryParse(v.ToString(), out var parsed) && parsed;
            }
            catch
            {
                return false;
            }
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            // 요구사항: 체크박스 선택 후 Print는 “라벨만 출력”
            //   - Grid 반영 X
            //   - CSV 저장 X
            //   - Count 반영 X
            PrintCheckedRows_LabelOnly_AndClearSelection();
        }

        private void PrintCheckedRows_LabelOnly_AndClearSelection()
        {
            bool printedAny = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (!IsRowChecked(row)) continue;

                var payload = (row.Cells["colTrayBarcode"]?.Value?.ToString() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(payload))
                    continue;

                TryPrintPayload_LabelOnly(payload);
                printedAny = true;
            }

            if (printedAny)
                ClearAllSelections();
        }

        private void ClearAllSelections()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                row.Cells[0].Value = false;
            }

            _selectAllChecked = false;
        }

        private void PrintSingleAuto()
        {
            EnsureTraySeqForToday();

            var now = DateTime.Now;
            var nowText = now.ToString("yyyy-MM-dd HH:mm:ss");
            int currentSeq = AutoLabelSequenceState.GetCurrentNo();
            var payload = BuildTrayBarcodeText(now, currentSeq);

            var result = TryPrintPayload(payload, currentSeq);
            // 요구사항: 최신 로그를 “맨 위”에 제한 없이 계속 쌓기
            dataGridView1.Rows.Insert(0, false, nowText, payload, result);

            if (TryGetDailySaveDirectory(now, out var dailyDir))
                SavePrintLogCsv(dailyDir, nowText, payload, result);

            UpdateTrayResult(currentSeq, result);

            if (string.Equals(result, "OK", StringComparison.OrdinalIgnoreCase))
            {
                AutoLabelSequenceState.SetCurrentNo(currentSeq + 1); _traySeq = currentSeq + 1;
                txtTrayBarcode.Text = payload;
            }

            UpdateCountsFromTrayResults();
        }

        private void TryPrintPayload_LabelOnly(string payload)
        {
            try
            {
                var printerName = (Properties.Settings.Default.PrinterName ?? "").Trim();
                if (string.IsNullOrWhiteSpace(printerName))
                {
                    MessageBox.Show(
                        "프린터 이름이 설정되지 않았습니다.\n(Setting에서 Windows에 설치된 프린터를 선택해 주세요.)",
                        "알림",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var zpl = BuildZplForPayload(payload);
                bool ok = RawPrinter.SendRawToPrinter(printerName, zpl);

                if (!ok)
                {
                    MessageBox.Show(
                        "프린터로 전송에 실패했습니다.\n(프린터 오프라인/연결/드라이버 상태를 확인하세요.)",
                        "오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"라벨 출력 중 오류가 발생했습니다.\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string TryPrintPayload(string payload, int autoSequenceNo)
        {
            try
            {
                var printerName = (Properties.Settings.Default.PrinterName ?? "").Trim();
                if (string.IsNullOrWhiteSpace(printerName))
                {
                    MessageBox.Show(
                        "프린터 이름이 설정되지 않았습니다.\n(Setting에서 Windows에 설치된 프린터를 선택해 주세요.)",
                        "알림",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return "NG";
                }

                var zpl = BuildZplForPayload(payload, autoSequenceNo, quantityOverride: 1);
                bool ok = RawPrinter.SendRawToPrinter(printerName, zpl);
                return ok ? "OK" : "NG";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"라벨 출력 중 오류가 발생했습니다.\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return "NG";
            }
        }

        private void UpdateCountLabels()
        {
            lblTotalCount.Text = _totalCount.ToString();
            lblOkCount.Text = _okCount.ToString();
            lblErrorCount.Text = _ngCount.ToString();

            lblErrorRatePer.Text =
                _totalCount == 0
                    ? "0.00 %"
                    : ((double)_ngCount / _totalCount * 100).ToString("0.00") + " %";
        }

        private void BtnReset_Click(object? sender, EventArgs e)
        {
            var ask = MessageBox.Show(
                "카운트와 로그를 초기화할까요?",
                "Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ask != DialogResult.Yes)
                return;

            ApplyTestDefaults();
            dataGridView1.Rows.Clear();

            _totalCount = 0;
            _okCount = 0;
            _ngCount = 0;
            UpdateCountLabels();

            if (_polling != null)
                _polling.ResetLatch();

            _traySeqDate = GetProductionDate(DateTime.Now); _traySeq = 1;
            AutoLabelSequenceState.SetCurrentNo(1);
            _trayResults.Clear();
            _maxTraySeq = 0;
            txtTrayBarcode.Text = "-";
            ClearAllSelections();
        }

        private bool TryGetDailySaveDirectory(DateTime now, out string dailyDir)
        {
            dailyDir = "";

            var basePath = (Properties.Settings.Default.SavePath ?? "").Trim();

            if (string.IsNullOrWhiteSpace(basePath))
            {
                MessageBox.Show(
                    "저장 경로가 설정되지 않았습니다.\nPath Setting에서 저장 경로를 먼저 설정해 주세요.",
                    "알림",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var productionDate = GetProductionDate(now);
                var dateFolder = productionDate.ToString("yyyyMMdd"); dailyDir = Path.Combine(basePath, dateFolder);

                if (!Directory.Exists(dailyDir))
                    Directory.CreateDirectory(dailyDir);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"저장 경로 처리 중 오류가 발생했습니다.\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private void SavePrintLogCsv(string dailyDir, string dateTime, string trayBarcode, string result)
        {
            var filePath = Path.Combine(dailyDir, "print_log.csv");

            try
            {
                if (!File.Exists(filePath))
                {
                    File.AppendAllText(
                        filePath,
                        "DateTime,TrayBarcode,Result\r\n",
                        Encoding.UTF8);
                }

                var sb = new StringBuilder();
                sb.Append(dateTime).Append(",");
                sb.Append(EscapeCsv(trayBarcode)).Append(",");
                sb.Append(EscapeCsv(result));
                sb.AppendLine();

                File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"로그 저장에 실패했습니다.\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RestoreTodayStateIfExists()
        {
            try
            {
                var basePath = (Properties.Settings.Default.SavePath ?? "").Trim();
                if (string.IsNullOrWhiteSpace(basePath))
                    return;

                var productionDate = GetProductionDate(DateTime.Now);
                var todayFolder = productionDate.ToString("yyyyMMdd");
                var dailyDir = Path.Combine(basePath, todayFolder);
                var csvPath = Path.Combine(dailyDir, "print_log.csv");

                if (!File.Exists(csvPath))
                {
                    ResetProductionDayState(productionDate, clearGrid: false);
                    return;
                }

                var lines = File.ReadAllLines(csvPath, Encoding.UTF8);
                if (lines.Length <= 1)
                {
                    ResetProductionDayState(productionDate, clearGrid: false);
                    return;
                }

                dataGridView1.Rows.Clear();

                _trayResults.Clear();
                _maxTraySeq = 0;

                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = ParseCsvLine(line);
                    if (cols.Length < 3)
                        continue;

                    var tray = cols[1];
                    var result = cols[2];

                    if (TryParseTraySeq(tray, out var seq))
                        UpdateTrayResult(seq, result);
                }

                // Keep grid newest-first for log view
                foreach (var line in lines.Skip(1).Reverse())
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = ParseCsvLine(line);
                    if (cols.Length < 3)
                        continue;

                    var dateTime = cols[0];
                    var tray = cols[1];
                    var result = cols[2];

                    dataGridView1.Rows.Add(false, dateTime, tray, result);
                }

                UpdateCountsFromTrayResults();

                _traySeqDate = productionDate;
                _traySeq = _maxTraySeq + 1;
                AutoLabelSequenceState.SetCurrentNo(_traySeq);

                if (_maxTraySeq > 0)
                    txtTrayBarcode.Text = $"TRAY{_maxTraySeq:0000}";
            }
            catch
            {
                // Ignore restore errors
            }
        }

        private static DateTime GetProductionDate(DateTime now)
        {
            var boundary = now.Date.AddHours(7);
            return now < boundary ? now.Date.AddDays(-1) : now.Date;
        }

        private void ResetProductionDayState(DateTime productionDate, bool clearGrid)
        {
            _traySeqDate = productionDate;
            _traySeq = 1;
            AutoLabelSequenceState.SetCurrentNo(1);

            _trayResults.Clear();
            _maxTraySeq = 0;
            _totalCount = 0;
            _okCount = 0;
            _ngCount = 0;
            UpdateCountLabels();

            if (clearGrid)
            {
                dataGridView1.Rows.Clear();
                ClearAllSelections();
            }

            txtTrayBarcode.Text = "-";
        }

        private static bool TryParseTraySeq(string trayBarcode, out int seq)
        {
            seq = 0;
            if (!trayBarcode.StartsWith("TRAY", StringComparison.OrdinalIgnoreCase))
                return false;

            if (trayBarcode.Length < 8)
                return false;

            return int.TryParse(trayBarcode.Substring(4, 4), out seq);
        }

        private void UpdateTrayResult(int traySeq, string result)
        {
            if (traySeq <= 0)
                return;

            _trayResults[traySeq] = result;
            if (traySeq > _maxTraySeq)
                _maxTraySeq = traySeq;
        }

        private void UpdateCountsFromTrayResults()
        {
            _totalCount = _maxTraySeq;
            _okCount = _trayResults.Values.Count(value =>
                string.Equals(value, "OK", StringComparison.OrdinalIgnoreCase));
            _ngCount = _trayResults.Values.Count(value =>
                !string.Equals(value, "OK", StringComparison.OrdinalIgnoreCase));
            UpdateCountLabels();
        }

        private static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            result.Add(sb.ToString());
            return result.ToArray();
        }

        private static string EscapeCsv(string text)
        {
            text ??= "";
            if (text.Contains(",") || text.Contains("\"") || text.Contains("\n"))
            {
                text = text.Replace("\"", "\"\"");
                return $"\"{text}\"";
            }
            return text;
        }

        private void ApplyGridStyle()
        {
            dataGridView1.EnableHeadersVisualStyles = false;

            var headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.False
            };

            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = headerStyle.Font;
            }

            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 40;

            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colDateTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            colTrayBarcode.SortMode = DataGridViewColumnSortMode.NotSortable;
            colResult.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void ApplyTestDefaults()
        {
            txtProductNum.Text = "TRAY-PRINT";
            txtProductName.Text = "DHS Potting Tray No.";
            txtType.Text = "10";
            txtCarModel.Text = "COMMON";
            txtALC.Text = "NFC TOUCH";
            txtBarcode.Text = "";

            txtTrayBarcode.Text = "-";

            lblTotalCount.Text = "0";
            lblOkCount.Text = "0";
            lblErrorCount.Text = "0";
            lblErrorRatePer.Text = "0.00 %";
        }

        private void Panel_LightBorder_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Control c) return;

            using var pen = new Pen(_lightBorderColor, 1);
            var r = c.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            e.Graphics.DrawRectangle(pen, r);
        }

        private void Grid_LightBorder_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Control c) return;

            using var pen = new Pen(_lightBorderColor, 1);
            var r = c.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            e.Graphics.DrawRectangle(pen, r);
        }

        private string BuildZplForPayload(string payload, int? startNoOverride = null, int? quantityOverride = null)
        {
            var model = PrintSettingStorage.Load();
            int startNo = startNoOverride ?? model.Print.StartNo;
            int quantity = quantityOverride ?? model.Print.Quantity;

            var resolved = LabelValueResolver.ApplyPlaceholders(model, payload, startNo, DateTime.Now, resolveNo: false);
            var zpl = LabelZplBuilder.Build(resolved, DefaultDpi, startNo, quantity);
            ZplDebugLogger.Dump("form-main", payload, zpl);
            return zpl;
        }
    }
}
