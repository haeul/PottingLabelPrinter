using System;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public partial class FormRunStartNo : Form
    {
        private readonly NumericUpDown nud;
        private readonly Button btnOk;
        private readonly Button btnCancel;

        public int SelectedNo => (int)nud.Value;

        public FormRunStartNo(int currentNo)
        {
            Text = "운전 시작 번호 설정";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Width = 320;
            Height = 160;

            var lbl = new Label
            {
                Text = "다음 출력 번호(운전용)를 입력하세요",
                AutoSize = true,
                Left = 16,
                Top = 16
            };

            nud = new NumericUpDown
            {
                Left = 16,
                Top = 44,
                Width = 270,
                Minimum = 1,
                Maximum = 999999,
                Increment = 1,
                Value = Math.Max(1, currentNo)
            };

            btnOk = new Button
            {
                Text = "확인",
                Left = 130,
                Top = 80,
                Width = 75,
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "취소",
                Left = 211,
                Top = 80,
                Width = 75,
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(lbl);
            Controls.Add(nud);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
