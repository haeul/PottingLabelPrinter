using System;
using System.IO;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public partial class FormPathSetting : Form
    {
        public FormPathSetting()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterParent;

            Load += FormPathSetting_Load;
            btnPathSave.Click += BtnPathSave_Click;
        }

        private void FormPathSetting_Load(object? sender, EventArgs e)
        {
            txtPath.Text = (Properties.Settings.Default.SavePath ?? "").Trim();
        }

        private void BtnPathSave_Click(object? sender, EventArgs e)
        {
            var basePath = (txtPath.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(basePath))
            {
                MessageBox.Show("저장 경로를 입력해 주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 기본 폴더 존재/생성
            if (!EnsureDirectory(basePath))
                return;

            // 날짜 폴더까지 만들어 두면, 메인에서 바로 쓰기 편하다.
            // (예: D:\logs\20260126\)
            var todayDir = GetDailyDirectory(basePath, DateTime.Now);
            if (!EnsureDirectory(todayDir))
                return;

            Properties.Settings.Default.SavePath = basePath;
            Properties.Settings.Default.Save();

            MessageBox.Show("저장 경로가 저장되었습니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        private static string GetDailyDirectory(string basePath, DateTime dt)
        {
            var dayFolder = dt.ToString("yyyyMMdd");
            return Path.Combine(basePath, dayFolder);
        }

        private static bool EnsureDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    return true;

                var ask = MessageBox.Show(
                    "입력한 폴더가 존재하지 않습니다.\n폴더를 생성할까요?",
                    "폴더 생성",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ask != DialogResult.Yes)
                    return false;

                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"폴더 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
