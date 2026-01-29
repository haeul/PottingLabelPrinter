using System;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public partial class FormPrintSetting : Form
    {
        public FormPrintSetting()
        {
            InitializeComponent();

            // 이벤트 연결
            btnSave.Click += BtnSave_Click;
            btnReset.Click += BtnReset_Click;
            btnPrint.Click += BtnPrint_Click;
        }

        private void FormPrintSetting_Load(object sender, EventArgs e)
        {
            // TODO: 추후 저장된 Print Setting 로드
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // TODO: 인쇄 설정 저장 로직
            MessageBox.Show(
                "Print setting saved (stub)",
                "Print Setting",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
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

            // TODO: 기본값으로 UI 초기화
            MessageBox.Show(
                "Print setting reset (stub)",
                "Print Setting",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            // TODO: 테스트 출력 로직
            MessageBox.Show(
                "Test print requested (stub)",
                "Print Setting",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
