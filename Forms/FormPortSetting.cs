using System;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public partial class FormPortSetting : Form
    {
        private bool _suppressEvents;

        public FormPortSetting()
        {
            InitializeComponent();

            cmbComBoard.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrinter.DropDownStyle = ComboBoxStyle.DropDownList;

            Load += FormPortSetting_Load;
            Activated += FormPortSetting_Activated;

            btnComBoard.Click += BtnComBoard_Click;
            btnPrinter.Click += BtnComPrinter_Click;
        }

        private void FormPortSetting_Load(object? sender, EventArgs e)
        {
            RefreshLists();
            ApplySavedSelection();
            UpdateUiState();
        }

        private void FormPortSetting_Activated(object? sender, EventArgs e)
        {
            var beforeBoard = cmbComBoard.SelectedItem?.ToString();
            var beforePrinter = cmbPrinter.SelectedItem?.ToString();

            RefreshLists();

            _suppressEvents = true;
            try
            {
                SelectComboByValue(cmbComBoard, beforeBoard ?? string.Empty);
                SelectComboByValue(cmbPrinter, beforePrinter ?? string.Empty);
            }
            finally
            {
                _suppressEvents = false;
            }

            ApplySavedSelection();
            UpdateUiState();
        }

        private void RefreshLists()
        {
            RefreshBoardPortList();
            RefreshPrinterList();
        }

        private void RefreshBoardPortList()
        {
            var ports = SerialPort.GetPortNames()
                                  .OrderBy(p => p)
                                  .ToArray();

            _suppressEvents = true;
            try
            {
                cmbComBoard.BeginUpdate();
                cmbComBoard.Items.Clear();

                if (ports.Length > 0)
                    cmbComBoard.Items.AddRange(ports);

                cmbComBoard.SelectedIndex = -1;
            }
            finally
            {
                cmbComBoard.EndUpdate();
                _suppressEvents = false;
            }
        }

        private void RefreshPrinterList()
        {
            var printers = PrinterSettings.InstalledPrinters
                                          .Cast<string>()
                                          .OrderBy(p => p)
                                          .ToArray();

            _suppressEvents = true;
            try
            {
                cmbPrinter.BeginUpdate();
                cmbPrinter.Items.Clear();

                if (printers.Length > 0)
                    cmbPrinter.Items.AddRange(printers);

                cmbPrinter.SelectedIndex = -1;
            }
            finally
            {
                cmbPrinter.EndUpdate();
                _suppressEvents = false;
            }
        }

        private void ApplySavedSelection()
        {
            var savedBoard = (Properties.Settings.Default.ComBoardPort ?? "").Trim();
            var savedPrinterName = (Properties.Settings.Default.PrinterName ?? "").Trim();

            _suppressEvents = true;
            try
            {
                SelectComboByValue(cmbComBoard, savedBoard);
                SelectComboByValue(cmbPrinter, savedPrinterName);
            }
            finally
            {
                _suppressEvents = false;
            }
        }

        private static void SelectComboByValue(ComboBox combo, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var idx = combo.Items.IndexOf(value);
            if (idx >= 0)
                combo.SelectedIndex = idx;
        }

        private void UpdateUiState()
        {
            var hasBoardPort = cmbComBoard.Items.Count > 0;
            var hasAnyPrinter = cmbPrinter.Items.Count > 0;

            btnComBoard.Enabled = hasBoardPort;
            btnPrinter.Enabled = hasAnyPrinter;

            if (!hasBoardPort && !hasAnyPrinter)
                Text = "Setting (포트/프린터 없음)";
            else if (!hasBoardPort)
                Text = "Setting (COM 포트 없음)";
            else if (!hasAnyPrinter)
                Text = "Setting (프린터 없음)";
            else
                Text = "Setting";
        }

        private void BtnComBoard_Click(object? sender, EventArgs e)
        {
            var port = cmbComBoard.SelectedItem?.ToString();

            if (!ValidateSelected(port, "COM Board"))
                return;

            Properties.Settings.Default.ComBoardPort = port!;
            Properties.Settings.Default.Save();

            MessageBox.Show("COM Board 포트가 저장되었습니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnComPrinter_Click(object? sender, EventArgs e)
        {
            var printerName = cmbPrinter.SelectedItem?.ToString();

            if (!ValidateSelected(printerName, "Printer"))
                return;

            Properties.Settings.Default.PrinterName = printerName!;
            Properties.Settings.Default.Save();

            MessageBox.Show("프린터가 저장되었습니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static bool ValidateSelected(string? value, string label)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show($"{label}를 선택해 주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
