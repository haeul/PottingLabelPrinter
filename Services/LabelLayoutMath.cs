using System;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class LabelLayoutMath
    {
        public static int MmToDotsInt(double mm, int dpi)
            => (int)Math.Round(mm * dpi / 25.4);

        public static double DotsToMm(int dots, int dpi)
            => dots * 25.4 / dpi;

        public static int SnapRotationToRightAngle(decimal rotation)
        {
            int rot = (int)Math.Round((double)rotation);
            rot = ((rot % 360) + 360) % 360;

            int[] allowed = { 0, 90, 180, 270 };
            int best = 0;
            int bestDiff = int.MaxValue;

            foreach (var a in allowed)
            {
                int diff = Math.Abs(rot - a);
                diff = Math.Min(diff, 360 - diff);
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    best = a;
                }
            }

            return best;
        }

        public static char RotationToZplOrientation(decimal rotation)
        {
            int snapped = SnapRotationToRightAngle(rotation);
            return snapped switch
            {
                90 => 'R',
                180 => 'I',
                270 => 'B',
                _ => 'N'
            };
        }

        public static char DirectionToZplOrientation(LabelPrintDirection direction)
        {
            return direction switch
            {
                LabelPrintDirection.Rotate90 => 'R',
                LabelPrintDirection.Rotate180 => 'I',
                LabelPrintDirection.Rotate270 => 'B',
                _ => 'N'
            };
        }

        // -------------------- DataMatrix SSOT --------------------

        public static int EstimateDmModulesFromData(string s)
        {
            int len = string.IsNullOrEmpty(s) ? 0 : s.Length;
            if (len <= 6) return 10;
            if (len <= 14) return 12;
            if (len <= 24) return 14;
            if (len <= 36) return 16;
            if (len <= 48) return 18;
            if (len <= 60) return 20;
            if (len <= 72) return 22;
            if (len <= 88) return 24;
            return Math.Min(144, 26 + (int)Math.Ceiling((len - 88) / 12.0));
        }

        /// <summary>
        /// DataMatrix 규칙(고정):
        /// - FontSizeMm = 목표 한 변(mm)
        /// - DM에서는 ScaleX/ScaleY를 무시한다.
        /// - modules = 데이터 길이 기반 추정
        /// - moduleDots = (targetSideMm / modules) mm->dots 양자화
        /// - sideDots = modules * moduleDots
        /// </summary>
        public static DmCalcResult CalcDataMatrixDots(LabelElement element, int dpi)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            string data = element.Value ?? "";
            int modules = EstimateDmModulesFromData(data);
            modules = Math.Max(10, modules);

            // scale 무시: 목표 한 변(mm) = FontSizeMm
            double targetSideMm = (double)element.FontSizeMm;
            targetSideMm = Math.Max(0.1, targetSideMm);

            double targetModuleMm = Math.Max(0.1, targetSideMm / modules);

            int moduleDots = Math.Max(1, MmToDotsInt(targetModuleMm, dpi));
            int sideDots = Math.Max(1, modules * moduleDots);

            double actualModuleMm = DotsToMm(moduleDots, dpi);
            double actualSideMm = modules * actualModuleMm;

            return new DmCalcResult
            {
                Modules = modules,
                ModuleDots = moduleDots,
                SideDots = sideDots,
                ActualSideMm = actualSideMm
            };
        }

        public sealed class DmCalcResult
        {
            public int Modules { get; set; }
            public int ModuleDots { get; set; }
            public int SideDots { get; set; }
            public double ActualSideMm { get; set; }
        }
    }
}
