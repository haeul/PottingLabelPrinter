using PottingLabelPrinter.Models;
using PottingLabelPrinter.Printer;
using PottingLabelPrinter.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public partial class FormPrintSetting : Form
    {
        private const int DefaultDpi = 203;
        private const double PreviewPaddingPx = 10.0;
        private PrintSettingModel _model = PrintSettingStorage.Load();
        private bool _isLoading;

        private const string ColNo = "No";
        private const string ColShowPreview = "ShowPreview";
        private const string ColShowPrint = "ShowPrint";
        private const string ColX = "Xmm";
        private const string ColY = "Ymm";
        private const string ColRotation = "Rotation";
        private const string ColFont = "FontSize";
        private const string ColScaleX = "ScaleX";
        private const string ColScaleY = "ScaleY";
        private const string ColValue = "Value";
        private const string ColType = "Type";

        public FormPrintSetting()
        {
            InitializeComponent();

            // 이벤트 연결
            btnSave.Click += BtnSave_Click;
            btnReset.Click += BtnReset_Click;
            btnPrint.Click += BtnPrint_Click;

            Load += FormPrintSetting_Load;
            pnlPreview.Paint += PnlPreview_Paint;
        }

        private void FormPrintSetting_Load(object sender, EventArgs e)
        {
            InitializeUi();
            ApplyModelToUi(_model);
            SetupGrid();
            ApplyElementsToGrid(_model.Elements);
            RefreshPreview();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var model = BuildModelFromUi();
                PrintSettingStorage.Save(model);
                _model = model;
                MessageBox.Show(
                    "Print setting saved.",
                    "Print Setting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Save failed: {ex.Message}",
                    "Print Setting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnReset_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Reset print settings to default?",
                "Print Setting",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            _model = PrintSettingStorage.Load();
            ApplyModelToUi(_model);
            ApplyElementsToGrid(_model.Elements);
            RefreshPreview();
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            var printerName = (Properties.Settings.Default.PrinterName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(printerName))
            {
                MessageBox.Show(
                    "Printer not selected. Set it in Port Setting.",
                    "Print Setting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Grid 편집 중이면 커밋 먼저
            CommitGridEdits();

            var model = BuildModelFromUi();
            var samplePayload = BuildSamplePayload();
            model = LabelValueResolver.ApplyPlaceholders(model, samplePayload, model.Print.StartNo, DateTime.Now, resolveNo: false);
            var zpl = LabelZplBuilder.Build(model, DefaultDpi);
            ZplDebugLogger.Dump("print-setting", samplePayload, zpl);

            bool ok = RawPrinter.SendRawToPrinter(printerName, zpl);

            MessageBox.Show(
                ok ? "Print command sent." : "Print failed.",
                "Print Setting",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );
        }

        private void InitializeUi()
        {
            _isLoading = true;

            cmbPrintDirection.Items.Clear();
            cmbPrintDirection.Items.AddRange(new object[]
            {
                LabelPrintDirection.Normal,
                LabelPrintDirection.Rotate90,
                LabelPrintDirection.Rotate180,
                LabelPrintDirection.Rotate270
            });

            ConfigureNumeric(nudPrintDarkness, 0, 30, 1, 0);
            ConfigureNumeric(nudPrintSpeed, 1, 10, 1, 0);
            ConfigureNumeric(nudPrintQuantity, 1, 9999, 1, 0);
            ConfigureNumeric(nudLabelStartNum, 1, 999999, 1, 0);

            ConfigureNumeric(nudLabelWidth, 1, 200, 0.1m, 1);
            ConfigureNumeric(nudLabelHeight, 1, 200, 0.1m, 1);
            ConfigureNumeric(nudLabelSpace, 0, 50, 0.1m, 1);
            ConfigureNumeric(nudXoffset, -50, 50, 0.1m, 1);
            ConfigureNumeric(nudYoffset, -50, 50, 0.1m, 1);

            // GapMm은 현재 ZPL에 적용하지 않으므로 비활성화하여 혼선을 줄인다.
            nudLabelSpace.Enabled = false;
            lblLabelSpace.Text = "라벨 간격";

            nudLabelWidth.ValueChanged += (_, __) => UpdateGridRangesFromLabel();
            nudLabelHeight.ValueChanged += (_, __) => UpdateGridRangesFromLabel();

            foreach (var numeric in new[] { nudPrintDarkness, nudPrintSpeed, nudPrintQuantity, nudLabelStartNum, nudLabelWidth, nudLabelHeight, nudLabelSpace, nudXoffset, nudYoffset })
            {
                numeric.ValueChanged += (_, __) => RefreshPreview();
            }

            cmbPrintDirection.SelectedIndexChanged += (_, __) => RefreshPreview();
            _isLoading = false;
        }

        private void ConfigureNumeric(NumericUpDown control, decimal min, decimal max, decimal increment, int decimals)
        {
            control.Minimum = min;
            control.Maximum = max;
            control.DecimalPlaces = decimals;
            control.Increment = increment;
        }

        private void SetupGrid()
        {
            dataGridView2.SuspendLayout();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.AllowUserToAddRows = true;
            dataGridView2.AllowUserToDeleteRows = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = ColNo,
                HeaderText = "순번",
                Width = 50,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // NEW: Preview/Print 체크박스
            dataGridView2.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = ColShowPreview,
                HeaderText = "미리보기",
                Width = 72,
                ThreeState = false
            });

            dataGridView2.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = ColShowPrint,
                HeaderText = "인쇄",
                Width = 54,
                ThreeState = false
            });

            dataGridView2.Columns.Add(BuildNumericColumn(ColX, "X(mm)", 0, 500, 0.1m, 1));
            dataGridView2.Columns.Add(BuildNumericColumn(ColY, "Y(mm)", 0, 500, 0.1m, 1));
            dataGridView2.Columns.Add(BuildRotationComboColumn());
            dataGridView2.Columns.Add(BuildNumericColumn(ColFont, "크기(mm)", 0.1m, 50, 0.1m, 1));
            dataGridView2.Columns.Add(BuildNumericColumn(ColScaleX, "X비율", 0.1m, 10, 0.1m, 2));
            dataGridView2.Columns.Add(BuildNumericColumn(ColScaleY, "Y비율", 0.1m, 10, 0.1m, 2));

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = ColValue,
                HeaderText = "데이터",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var typeColumn = new DataGridViewComboBoxColumn
            {
                Name = ColType,
                HeaderText = "타입",
                Width = 80,
                DataSource = Enum.GetValues(typeof(LabelElementType))
            };
            dataGridView2.Columns.Add(typeColumn);

            // ---- 즉시 반영(CommitEdit 흐름) ----
            dataGridView2.CurrentCellDirtyStateChanged += DataGridView2_CurrentCellDirtyStateChanged;
            dataGridView2.CellValueChanged += DataGridView2_CellValueChanged;

            dataGridView2.CellEndEdit += (_, __) => RefreshPreview();
            dataGridView2.RowsAdded += (_, __) => UpdateRowNumbers();
            dataGridView2.RowsRemoved += (_, __) => UpdateRowNumbers();
            dataGridView2.DataError += (s, e) =>
            {
                // 최소한 Debug로라도 원인 남겨두기
                System.Diagnostics.Debug.WriteLine($"Grid DataError: {e.Exception?.Message}");
                e.ThrowException = false;
            };

            dataGridView2.ResumeLayout();
            UpdateGridRangesFromLabel();
        }

        private DataGridViewComboBoxColumn BuildRotationComboColumn()
        {
            var col = new DataGridViewComboBoxColumn
            {
                Name = ColRotation,
                HeaderText = "회전",
                Width = 70,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                ValueType = typeof(decimal),   // 핵심
            };

            col.Items.Add(0m);
            col.Items.Add(90m);
            col.Items.Add(180m);
            col.Items.Add(270m);

            return col;
        }


        private void DataGridView2_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (_isLoading) return;
            if (!dataGridView2.IsCurrentCellDirty) return;

            // 체크박스/콤보박스/편집 컨트롤 값 변경 즉시 커밋
            dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridView2_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (_isLoading) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 값 변경 즉시 미리보기 갱신
            RefreshPreview();
        }

        private LabelNumericColumn BuildNumericColumn(string name, string header, decimal min, decimal max, decimal increment, int decimals)
        {
            return new LabelNumericColumn
            {
                Name = name,
                HeaderText = header,
                Width = 80,
                Minimum = min,
                Maximum = max,
                Increment = increment,
                DecimalPlaces = decimals
            };
        }

        private void UpdateGridRangesFromLabel()
        {
            decimal width = nudLabelWidth.Value;
            decimal height = nudLabelHeight.Value;

            if (dataGridView2.Columns[ColX] is LabelNumericColumn colX)
                colX.Maximum = width;
            if (dataGridView2.Columns[ColY] is LabelNumericColumn colY)
                colY.Maximum = height;
            if (dataGridView2.Columns[ColFont] is LabelNumericColumn colFont)
                colFont.Maximum = Math.Max(width, height);
        }

        private void ApplyModelToUi(PrintSettingModel model)
        {
            _isLoading = true;

            cmbPrintDirection.SelectedItem = model.PrintDirection;
            if (cmbPrintDirection.SelectedItem == null && cmbPrintDirection.Items.Count > 0)
                cmbPrintDirection.SelectedIndex = 0;

            nudPrintDarkness.Value = Clamp(nudPrintDarkness, model.Print.Darkness);
            nudPrintSpeed.Value = Clamp(nudPrintSpeed, model.Print.Speed);
            nudPrintQuantity.Value = Clamp(nudPrintQuantity, model.Print.Quantity);
            nudLabelStartNum.Value = Clamp(nudLabelStartNum, model.Print.StartNo);

            nudLabelWidth.Value = Clamp(nudLabelWidth, model.Geometry.LabelWidthMm);
            nudLabelHeight.Value = Clamp(nudLabelHeight, model.Geometry.LabelHeightMm);
            nudLabelSpace.Value = Clamp(nudLabelSpace, model.Geometry.GapMm);
            nudXoffset.Value = Clamp(nudXoffset, model.Geometry.OffsetXmm);
            nudYoffset.Value = Clamp(nudYoffset, model.Geometry.OffsetYmm);

            _isLoading = false;
        }

        private decimal Clamp(NumericUpDown control, decimal value)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }

        private void ApplyElementsToGrid(List<LabelElement> elements)
        {
            dataGridView2.Rows.Clear();
            if (elements == null || elements.Count == 0)
                elements = PrintSettingModel.CreateDefault().Elements;

            foreach (var element in elements)
            {
                var rowIndex = dataGridView2.Rows.Add();
                var row = dataGridView2.Rows[rowIndex];

                row.Cells[ColShowPreview].Value = element.ShowPreview;
                row.Cells[ColShowPrint].Value = element.ShowPrint;

                row.Cells[ColX].Value = element.Xmm;
                row.Cells[ColY].Value = element.Ymm;
                row.Cells[ColRotation].Value = (decimal)SnapRotationToComboValue(element.Rotation);
                row.Cells[ColFont].Value = element.FontSizeMm;
                row.Cells[ColScaleX].Value = element.ScaleX;
                row.Cells[ColScaleY].Value = element.ScaleY;
                row.Cells[ColValue].Value = element.Value;
                row.Cells[ColType].Value = element.Type;
            }

            UpdateRowNumbers();
        }

        private decimal SnapRotationToComboValue(decimal rotation)
        {
            int v = (int)Math.Round(rotation);

            v %= 360;
            if (v < 0) v += 360;

            if (v >= 315 || v < 45) return 0m;
            if (v < 135) return 90m;
            if (v < 225) return 180m;
            return 270m;
        }


        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                var row = dataGridView2.Rows[i];
                if (row.IsNewRow)
                    continue;

                row.Cells[ColNo].Value = (i + 1).ToString();

                // 기본값 보정
                if (row.Cells[ColType].Value == null)
                    row.Cells[ColType].Value = LabelElementType.Text;

                if (row.Cells[ColShowPreview].Value == null)
                    row.Cells[ColShowPreview].Value = true;

                if (row.Cells[ColShowPrint].Value == null)
                    row.Cells[ColShowPrint].Value = true;

                if (row.Cells[ColRotation].Value == null)
                    row.Cells[ColRotation].Value = 0;

            }
        }

        private PrintSettingModel BuildModelFromUi()
        {
            var model = new PrintSettingModel
            {
                PrintDirection = cmbPrintDirection.SelectedItem is LabelPrintDirection direction
                    ? direction
                    : LabelPrintDirection.Normal,
                Print = new PrintRuntimeSettings
                {
                    Darkness = (int)nudPrintDarkness.Value,
                    Speed = (int)nudPrintSpeed.Value,
                    Quantity = (int)nudPrintQuantity.Value,
                    StartNo = (int)nudLabelStartNum.Value
                },
                Geometry = new LabelGeometrySettings
                {
                    LabelWidthMm = nudLabelWidth.Value,
                    LabelHeightMm = nudLabelHeight.Value,
                    GapMm = nudLabelSpace.Value,
                    OffsetXmm = nudXoffset.Value,
                    OffsetYmm = nudYoffset.Value
                },
                Elements = BuildElementsFromGrid()
            };

            return model;
        }

        private List<LabelElement> BuildElementsFromGrid()
        {
            var elements = new List<LabelElement>();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.IsNewRow) continue;

                elements.Add(new LabelElement
                {
                    No = elements.Count + 1,

                    ShowPreview = ReadBool(row.Cells[ColShowPreview].Value, true),
                    ShowPrint = ReadBool(row.Cells[ColShowPrint].Value, true),

                    Xmm = ReadDecimal(row.Cells[ColX].Value),
                    Ymm = ReadDecimal(row.Cells[ColY].Value),
                    Rotation = ReadDecimal(row.Cells[ColRotation].Value),
                    FontSizeMm = ReadDecimal(row.Cells[ColFont].Value, 2.6m),
                    ScaleX = ReadDecimal(row.Cells[ColScaleX].Value, 1m),
                    ScaleY = ReadDecimal(row.Cells[ColScaleY].Value, 1m),
                    Value = (row.Cells[ColValue].Value ?? "").ToString() ?? "",
                    Type = row.Cells[ColType].Value is LabelElementType type ? type : LabelElementType.Text
                });
            }

            return elements;
        }

        private bool ReadBool(object? value, bool fallback)
        {
            if (value == null) return fallback;
            if (value is bool b) return b;
            if (bool.TryParse(value.ToString(), out var parsed)) return parsed;
            return fallback;
        }

        private decimal ReadDecimal(object? value, decimal fallback = 0m)
        {
            if (value == null) return fallback;
            if (value is decimal d) return d;
            if (value is double dbl) return (decimal)dbl;
            if (decimal.TryParse(value.ToString(), out var parsed)) return parsed;
            return fallback;
        }

        private void RefreshPreview()
        {
            if (_isLoading) return;
            pnlPreview.Invalidate();
        }

        private void CommitGridEdits()
        {
            // 현재 편집 중이면 커밋해서 BuildModelFromUi가 최신 값을 읽게 함
            if (dataGridView2.IsCurrentCellInEditMode)
            {
                dataGridView2.EndEdit();
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void PnlPreview_Paint(object? sender, PaintEventArgs e)
        {
            // 미리보기는 “현재 편집 중 값”도 반영되도록 커밋 후 모델 생성
            CommitGridEdits();

            var model = BuildModelFromUi();
            var samplePayload = BuildSamplePayload();
            model = LabelValueResolver.ApplyPlaceholders(model, samplePayload, model.Print.StartNo, DateTime.Now, resolveNo: true);
            LabelPreviewRenderer.DrawPreview(e.Graphics, pnlPreview.ClientRectangle, model, DefaultDpi, PreviewPaddingPx);
        }

        private string BuildSamplePayload()
        {
            var now = DateTime.Now;
            // 핵심: 0000 같은 고정 숫자 대신 {NO}를 남긴다
            return $"TRAY{{NO}} {now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
