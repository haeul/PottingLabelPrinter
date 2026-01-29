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

            var model = BuildModelFromUi();
            var zpl = LabelZplBuilder.Build(model, DefaultDpi);
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

            dataGridView2.Columns.Add(BuildNumericColumn(ColX, "X(mm)", 0, 500, 0.1m, 1));
            dataGridView2.Columns.Add(BuildNumericColumn(ColY, "Y(mm)", 0, 500, 0.1m, 1));
            dataGridView2.Columns.Add(BuildNumericColumn(ColRotation, "회전", 0, 360, 1m, 0));
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

            dataGridView2.CellEndEdit += (_, __) => RefreshPreview();
            dataGridView2.RowsAdded += (_, __) => UpdateRowNumbers();
            dataGridView2.RowsRemoved += (_, __) => UpdateRowNumbers();
            dataGridView2.DataError += (_, __) => { };

            dataGridView2.ResumeLayout();
            UpdateGridRangesFromLabel();
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
                row.Cells[ColX].Value = element.Xmm;
                row.Cells[ColY].Value = element.Ymm;
                row.Cells[ColRotation].Value = element.Rotation;
                row.Cells[ColFont].Value = element.FontSizeMm;
                row.Cells[ColScaleX].Value = element.ScaleX;
                row.Cells[ColScaleY].Value = element.ScaleY;
                row.Cells[ColValue].Value = element.Value;
                row.Cells[ColType].Value = element.Type;
            }

            UpdateRowNumbers();
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                var row = dataGridView2.Rows[i];
                if (!row.IsNewRow)
                {
                    row.Cells[ColNo].Value = (i + 1).ToString();
                    if (row.Cells[ColType].Value == null)
                        row.Cells[ColType].Value = LabelElementType.Text;
                }
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

        private void PnlPreview_Paint(object? sender, PaintEventArgs e)
        {
            var model = BuildModelFromUi();
            LabelPreviewRenderer.DrawPreview(e.Graphics, pnlPreview.ClientRectangle, model, DefaultDpi, PreviewPaddingPx);
        }
    }
}