using System;
using System.IO;
using System.Text.Json;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// 운전용(장비 신호 기반) 시퀀스 번호를 파일로 영구 저장.
    /// PrintSettingStorage와 동일한 SavePath 정책을 재사용한다.
    /// </summary>
    public static class LabelSequenceStorage
    {
        private const string FileName = "sequence-state.json";

        private class SequenceDto
        {
            public int CurrentNo { get; set; } = 1;  // "다음에 출력할 번호"
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        }

        private static string GetPath()
        {
            // PrintSettingStorage의 저장 폴더 정책을 그대로 사용
            // (MyDocuments\PottingLabelPrinter 또는 사용자가 지정한 SavePath)
            string settingPath = PrintSettingStorage.GetSettingsPath();
            string baseDir = Path.GetDirectoryName(settingPath) ?? Environment.CurrentDirectory;
            Directory.CreateDirectory(baseDir);
            return Path.Combine(baseDir, FileName);
        }

        public static int LoadCurrentNo()
        {
            var path = GetPath();
            if (!File.Exists(path))
                return 1;

            try
            {
                var json = File.ReadAllText(path);
                var dto = JsonSerializer.Deserialize<SequenceDto>(json);
                if (dto == null) return 1;
                return Math.Max(1, dto.CurrentNo);
            }
            catch
            {
                return 1;
            }
        }

        public static void SaveCurrentNo(int currentNo)
        {
            currentNo = Math.Max(1, currentNo);

            var path = GetPath();
            var dto = new SequenceDto
            {
                CurrentNo = currentNo,
                UpdatedAt = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }
    }
}
