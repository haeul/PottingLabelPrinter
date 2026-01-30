using System;
using System.IO;
namespace PottingLabelPrinter.Services
{
    public static class ZplDebugLogger
    {
        private const string FileName = "last-zpl.txt";

        public static void Dump(string context, string payload, string zpl)
        {
            if (string.IsNullOrWhiteSpace(zpl))
                return;

            try
            {
                var basePath = PrintSettingStorage.GetSettingsPath();
                var dir = Path.GetDirectoryName(basePath) ?? Environment.CurrentDirectory;
                Directory.CreateDirectory(dir);

                var path = Path.Combine(dir, FileName);
                var header = $"# {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {context}\r\n# Payload: {payload}\r\n";
                File.WriteAllText(path, header + zpl);
            }
            catch
            {
                // 디버그 로그 실패는 출력 동작에 영향을 주지 않음
            }
        }
    }
}