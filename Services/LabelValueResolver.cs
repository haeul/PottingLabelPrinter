// LabelValueResolver.cs
using System;
using System.Collections.Generic;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class LabelValueResolver
    {
        public static PrintSettingModel ApplyPlaceholders(
            PrintSettingModel model,
            string payload,
            int currentNo,
            DateTime? now = null,
            bool resolveNo = true)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var resolved = CloneModel(model);

            string trimmed = (payload ?? string.Empty).Trim();
            string tray = "-";
            string date = "-";
            string time = "-";

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                var parts = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1) tray = parts[0];
                if (parts.Length >= 2) date = parts[1];
                if (parts.Length >= 3) time = parts[2];
            }

            DateTime resolvedNow = now ?? DateTime.Now;
            if (date == "-") date = resolvedNow.ToString("yyyy-MM-dd");
            if (time == "-") time = resolvedNow.ToString("HH:mm:ss");

            // 핵심: Preview에서 {NO}도 Print처럼 4자리 패딩 규칙 통일
            string noText = Math.Max(0, currentNo).ToString("0000");

            foreach (var element in resolved.Elements)
            {
                string value = element.Value ?? string.Empty;
                if (element.Type == LabelElementType.DataMatrix && string.IsNullOrWhiteSpace(value))
                {
                    value = trimmed;
                }

                value = value.Replace("{PAYLOAD}", trimmed)
                             .Replace("{TRAY}", tray)
                             .Replace("{DATE}", date)
                             .Replace("{TIME}", time);

                if (resolveNo)
                    value = value.Replace("{NO}", noText);   // <-- 변경 포인트

                element.Value = value;
            }

            return resolved;
        }

        private static PrintSettingModel CloneModel(PrintSettingModel model)
        {
            var cloned = new PrintSettingModel
            {
                PrintDirection = model.PrintDirection,
                Print = new PrintRuntimeSettings
                {
                    Darkness = model.Print.Darkness,
                    Speed = model.Print.Speed,
                    Quantity = model.Print.Quantity,
                    StartNo = model.Print.StartNo
                },
                Geometry = new LabelGeometrySettings
                {
                    LabelWidthMm = model.Geometry.LabelWidthMm,
                    LabelHeightMm = model.Geometry.LabelHeightMm,
                    GapMm = model.Geometry.GapMm,
                    OffsetXmm = model.Geometry.OffsetXmm,
                    OffsetYmm = model.Geometry.OffsetYmm
                },
                Elements = new List<LabelElement>()
            };

            foreach (var element in model.Elements)
            {
                cloned.Elements.Add(new LabelElement
                {
                    Key = element.Key,
                    No = element.No,
                    Type = element.Type,
                    ShowPreview = element.ShowPreview,
                    ShowPrint = element.ShowPrint,
                    Xmm = element.Xmm,
                    Ymm = element.Ymm,
                    Rotation = element.Rotation,
                    FontSizeMm = element.FontSizeMm,
                    ScaleX = element.ScaleX,
                    ScaleY = element.ScaleY,
                    Value = element.Value ?? string.Empty
                });
            }

            return cloned;
        }
    }
}
