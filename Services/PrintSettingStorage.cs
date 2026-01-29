using System;
using System.IO;
using System.Text.Json;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class PrintSettingStorage
    {
        private const string FileName = "print-settings.json";

        public static string GetSettingsPath()
        {
            var basePath = (Properties.Settings.Default.SavePath ?? "").Trim();
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "PottingLabelPrinter");
            }

            Directory.CreateDirectory(basePath);
            return Path.Combine(basePath, FileName);
        }

        public static PrintSettingModel Load()
        {
            var path = GetSettingsPath();
            if (!File.Exists(path))
                return PrintSettingModel.CreateDefault();

            try
            {
                var json = File.ReadAllText(path);
                var model = JsonSerializer.Deserialize<PrintSettingModel>(json);
                return model ?? PrintSettingModel.CreateDefault();
            }
            catch
            {
                return PrintSettingModel.CreateDefault();
            }
        }

        public static void Save(PrintSettingModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var path = GetSettingsPath();
            var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(path, json);
        }
    }
}
