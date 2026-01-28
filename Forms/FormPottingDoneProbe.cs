using PottingLabelPrinter.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    /// <summary>
    /// [테스트용] FC 0x02(Discrete Inputs)에서 IN2 완료신호의 ON 유지시간을 측정하는 폼.
    /// - readDone: 완료신호(bool) 읽기 콜백 (IN2)
    /// - readRawInputsByte: (선택) FC02 응답 Data 1바이트(비트맵) 읽기 콜백
    /// - intervalMs: 폴링 주기(ms) - 기본 200ms
    /// </summary>
    public partial class FormDoneProbe : System.Windows.Forms.Form
    {
        private readonly Func<bool> _readDone;
        private readonly Func<byte>? _readRawInputsByte;
        private readonly int _intervalMs;

        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        private bool _prevDone;
        private DateTime? _riseAt;
        private long? _lastDurationMs;
        private ModbusPollingService? _polling;

        private readonly List<string> _log = new List<string>(256);

        public FormDoneProbe(
            Func<bool> readDone,
            Func<byte>? readRawInputsByte = null,
            int intervalMs = 200,
            string? titleSuffix = null)
        {
            _readDone = readDone ?? throw new ArgumentNullException(nameof(readDone));
            _readRawInputsByte = readRawInputsByte;
            _intervalMs = Math.Max(20, intervalMs);

            InitializeComponent();

            Text = $"Done Probe{(string.IsNullOrWhiteSpace(titleSuffix) ? "" : $" - {titleSuffix}")}";
            lblInterval.Text = $"Interval: {_intervalMs} ms";

            _timer.Interval = _intervalMs;
            _timer.Tick += (_, __) => PollOnce();

            btnCopy.Click += (_, __) => CopyAllToClipboard();
            btnClear.Click += (_, __) => ClearLog();
            btnClose.Click += (_, __) => Close();

            Shown += (_, __) =>
            {
                _prevDone = SafeReadDone();
                AppendLog($"INIT done={_prevDone}");
                UpdateUi(DateTime.Now, _prevDone, SafeReadRaw());
                _timer.Start();
            };

            FormClosing += (_, __) =>
            {
                try { _timer.Stop(); } catch { /* ignore */ }
            };
        }

        private void PollOnce()
        {
            var now = DateTime.Now;

            bool done = SafeReadDone();
            byte raw = SafeReadRaw();

            if (done && !_prevDone)
            {
                _riseAt = now;
                AppendLog($"RISE  done=true   raw=0x{raw:X2} bin={ToBin8(raw)}  at {now:HH:mm:ss.fff}");
            }
            else if (!done && _prevDone)
            {
                long? durMs = null;
                if (_riseAt.HasValue)
                {
                    durMs = (long)Math.Max(0, (now - _riseAt.Value).TotalMilliseconds);
                    _lastDurationMs = durMs;
                }

                AppendLog($"FALL  done=false  raw=0x{raw:X2} bin={ToBin8(raw)}  at {now:HH:mm:ss.fff}" +
                          (durMs.HasValue ? $"  duration={durMs.Value} ms" : "  duration=-(no riseAt)"));

                _riseAt = null;
            }

            _prevDone = done;
            UpdateUi(now, done, raw);
        }

        private bool SafeReadDone()
        {
            try { return _readDone(); }
            catch (Exception ex)
            {
                AppendLog($"ERR readDone: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        private byte SafeReadRaw()
        {
            if (_readRawInputsByte == null) return 0;

            try { return _readRawInputsByte(); }
            catch (Exception ex)
            {
                AppendLog($"ERR readRaw: {ex.GetType().Name}: {ex.Message}");
                return 0;
            }
        }

        private void UpdateUi(DateTime now, bool done, byte raw)
        {
            lblNow.Text = $"Now: {now:yyyy-MM-dd HH:mm:ss.fff}";
            lblDone.Text = $"IN2 Done: {(done ? "ON" : "OFF")}";

            if (_readRawInputsByte != null)
                lblRaw.Text = $"Raw(byte): 0x{raw:X2}  (bin: {ToBin8(raw)})";
            else
                lblRaw.Text = "Raw(byte): -  (bin: -)";

            lblRiseAt.Text = _riseAt.HasValue
                ? $"Last RISE: {_riseAt.Value:yyyy-MM-dd HH:mm:ss.fff}"
                : "Last RISE: -";

            lblFallAt.Text = TryGetLastFallLine(out var fallLine)
                ? $"Last FALL: {fallLine}"
                : "Last FALL: -";

            lblDuration.Text = _lastDurationMs.HasValue
                ? $"Last Duration(ms): {_lastDurationMs.Value}"
                : "Last Duration(ms): -";
        }

        private bool TryGetLastFallLine(out string line)
        {
            for (int i = _log.Count - 1; i >= 0; i--)
            {
                if (_log[i].StartsWith("FALL", StringComparison.OrdinalIgnoreCase))
                {
                    line = _log[i];
                    return true;
                }
            }
            line = "";
            return false;
        }

        private void AppendLog(string message)
        {
            const int MaxLines = 200;

            _log.Add(message);
            if (_log.Count > MaxLines)
                _log.RemoveRange(0, _log.Count - MaxLines);

            listLog.BeginUpdate();
            try
            {
                listLog.Items.Clear();
                foreach (var s in _log)
                    listLog.Items.Add(s);

                if (listLog.Items.Count > 0)
                    listLog.TopIndex = listLog.Items.Count - 1;
            }
            finally
            {
                listLog.EndUpdate();
            }
        }

        private void ClearLog()
        {
            _log.Clear();
            listLog.Items.Clear();
            _riseAt = null;
            _lastDurationMs = null;
            AppendLog("CLEARED");
        }

        private void CopyAllToClipboard()
        {
            var sb = new StringBuilder();
            sb.AppendLine(lblNow.Text);
            sb.AppendLine(lblInterval.Text);
            sb.AppendLine(lblDone.Text);
            sb.AppendLine(lblRaw.Text);
            sb.AppendLine(lblRiseAt.Text);
            sb.AppendLine(lblFallAt.Text);
            sb.AppendLine(lblDuration.Text);
            sb.AppendLine();
            sb.AppendLine("---- LOG ----");
            foreach (var s in _log)
                sb.AppendLine(s);

            try
            {
                Clipboard.SetText(sb.ToString());
                AppendLog("COPIED to clipboard");
            }
            catch (Exception ex)
            {
                AppendLog($"ERR Clipboard: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static string ToBin8(byte b)
        {
            char[] chars = new char[8];
            for (int i = 7; i >= 0; i--)
            {
                int bit = (b >> i) & 1;
                chars[7 - i] = bit == 1 ? '1' : '0';
            }
            return new string(chars);
        }

        private void uiTimer_Tick(object sender, EventArgs e)
        {
            lblCommStatus.Text = _polling.LastCommStatus;
            lblCommError.Text = _polling.LastCommError;
            lblLastCommAt.Text = _polling.LastCommAt.ToString("HH:mm:ss.fff");
        }
    }
}
