// LabelZplBuilder.cs
using System;
using System.Collections.Generic;
using System.Text;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class LabelZplBuilder
    {
        private const double NudgeYmm = 0.6; // 출력 미세 보정

        public static string Build(PrintSettingModel model, int dpi)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            int qty = Math.Max(1, model.Print.Quantity);
            int startNo = Math.Max(1, model.Print.StartNo);

            var sb = new StringBuilder();
            for (int i = 0; i < qty; i++)
            {
                int currentNo = startNo + i;
                sb.Append(BuildSingle(model, dpi, currentNo));
            }

            return sb.ToString();
        }

        private static string BuildSingle(PrintSettingModel model, int dpi, int currentNo)
        {
            int pw = MmToDots((double)model.Geometry.LabelWidthMm, dpi);

            // A안: 라벨 간격(GapMm)은 ZPL에 반영하지 않는다.
            // (GapMm을 넣으면 ^LL(피드 길이)이 늘어나서 출력 결과가 달라질 수 있음)
            double labelLengthMm = (double)model.Geometry.LabelHeightMm;
            int ll = MmToDots(labelLengthMm, dpi);

            var sb = new StringBuilder(512);
            sb.AppendLine("~SD" + Clamp(model.Print.Darkness, 0, 30));
            sb.AppendLine("^XA");
            sb.AppendLine("^PW" + pw);
            sb.AppendLine("^LL" + ll);
            sb.AppendLine("^LH0,0");
            sb.AppendLine("^LT" + MmToDots(NudgeYmm, dpi));
            sb.AppendLine("^LS0");
            sb.AppendLine("^PR" + Clamp(model.Print.Speed, 1, 10));
            sb.AppendLine("^PO" + ToOrientation(model.PrintDirection));
            sb.AppendLine("^PQ1");

            foreach (var element in model.Elements ?? new List<LabelElement>())
            {
                if (!element.ShowPrint) continue;
                AppendElement(sb, element, model.Geometry, dpi, currentNo);
            }

            sb.AppendLine("^XZ");
            return sb.ToString();
        }

        private static void AppendElement(StringBuilder sb, LabelElement element, LabelGeometrySettings geometry, int dpi, int currentNo)
        {
            decimal xMm = element.Xmm + geometry.OffsetXmm;
            decimal yMm = element.Ymm + geometry.OffsetYmm;

            int x = MmToDots((double)xMm, dpi);
            int y = MmToDots((double)(yMm + (decimal)NudgeYmm), dpi);
            char orientation = ToOrientation(element.Rotation);

            string value = (element.Value ?? string.Empty).Replace("{NO}", currentNo.ToString());
            value = Escape(value);

            if (element.Type == LabelElementType.DataMatrix)
            {
                // SSOT: FontSizeMm = 목표 한 변(mm), ScaleX/ScaleY 무시
                var dm = LabelLayoutMath.CalcDataMatrixDots(element, dpi);
                int moduleDots = Math.Max(1, dm.ModuleDots);

                sb.AppendLine($"^FO{x},{y}^BX{orientation},{moduleDots},200^FD{value}^FS");
                return;
            }

            int h = Math.Max(1, MmToDots((double)(element.FontSizeMm * element.ScaleY), dpi));
            int w = Math.Max(1, (int)Math.Round(h * (double)element.ScaleX));
            sb.AppendLine($"^FO{x},{y}^A0{orientation},{h},{w}^FD{value}^FS");
        }

        private static char ToOrientation(LabelPrintDirection direction)
        {
            return direction switch
            {
                LabelPrintDirection.Rotate90 => 'R',
                LabelPrintDirection.Rotate180 => 'I',
                LabelPrintDirection.Rotate270 => 'B',
                _ => 'N'
            };
        }

        private static char ToOrientation(decimal rotation)
        {
            int rot = (int)Math.Round(rotation);
            rot = ((rot % 360) + 360) % 360;
            return rot switch
            {
                90 => 'R',
                180 => 'I',
                270 => 'B',
                _ => 'N'
            };
        }

        private static int MmToDots(double mm, int dpi)
            => LabelLayoutMath.MmToDotsInt(mm, dpi);

        private static string Escape(string value)
        {
            return value.Replace("^", "").Replace("~", "");
        }

        private static int Clamp(int value, int min, int max)
            => Math.Min(Math.Max(value, min), max);
    }
}
