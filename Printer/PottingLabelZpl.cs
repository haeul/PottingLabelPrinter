using System;
using System.Text;

namespace PottingLabelPrinter.Printer
{
    /// <summary>
    /// 포팅 라벨 전용 ZPL 빌더.
    /// 현장 라벨이 고정이므로 복잡한 옵션 없이 "항상 같은 레이아웃"으로 만든다.
    /// </summary>
    public static class PottingLabelZpl
    {
        // ZD421은 보통 203dpi(8dpmm) 모델이 많다.
        // 300dpi 모델이면 결과가 달라지므로 필요 시 설정값으로 빼면 된다.
        public const int DefaultDpi = 203;

        // 라벨(mm)
        private const double LabelWmm = 44.0;
        private const double LabelHmm = 14.0;

        // DataMatrix(mm) 및 위치(mm)
        private const double DmSideMm = 7.0;
        private const double DmLeftMm = 5.0;
        private const double DmTopMm = 4.0;

        // 텍스트 규격(mm)
        private const double TextHeightMm = 2.0;

        // 텍스트 위치: DM 오른쪽으로 약간 띄운다.
        private const double TextLeftMm = DmLeftMm + DmSideMm + 1.0; // DM 오른쪽 + 1mm
        private const double TextLine1TopMm = 3.6;                  // 사진처럼 위쪽 정렬 조금 올림
        private const double TextLineGapMm = 0.9;                   // 줄 간격(현장 라벨 느낌)

        /// <summary>
        /// payload 예: "TRAY0074 2026-01-21 17:01:24"
        /// - 1줄: TRAY0074
        /// - 2줄: 2026-01-21
        /// - 3줄: 17:01:24
        /// - DataMatrix: payload 전체를 넣는다.
        /// </summary>
        public static string Build(string payload, int dpi = DefaultDpi)
        {
            payload ??= "";
            payload = payload.Trim();

            // 텍스트 3줄로 분리 (스페이스 기준)
            string line1 = "-";
            string line2 = "-";
            string line3 = "-";

            if (!string.IsNullOrWhiteSpace(payload))
            {
                var parts = payload.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 1) line1 = parts[0]; // TRAY0074
                if (parts.Length >= 2) line2 = parts[1]; // 2026-01-21
                if (parts.Length >= 3) line3 = parts[2]; // 17:01:24
            }

            int pw = MmToDots(LabelWmm, dpi);
            int ll = MmToDots(LabelHmm, dpi);

            int dmX = MmToDots(DmLeftMm, dpi);
            int dmY = MmToDots(DmTopMm, dpi);

            // DataMatrix 모듈 크기 (현장 라벨과 유사하게 맞추기 쉬운 기본값)
            int dmModuleDots = 3;

            // 텍스트 폰트 높이 (2mm)
            int textH = MmToDots(TextHeightMm, dpi);
            int textW = textH;

            int tX = MmToDots(TextLeftMm, dpi);

            // 3줄 Y 좌표 계산
            int t1Y = MmToDots(TextLine1TopMm, dpi);
            int t2Y = MmToDots(TextLine1TopMm + TextHeightMm + TextLineGapMm, dpi);
            int t3Y = MmToDots(TextLine1TopMm + (TextHeightMm + TextLineGapMm) * 2.0, dpi);

            var sb = new StringBuilder(512);

            sb.AppendLine("^XA");
            sb.AppendLine("^PW352");
            sb.AppendLine("^LL112");

            // Data Matrix (payload 전체)
            sb.AppendLine("^FX");
            sb.AppendLine("^FO80,30");
            sb.AppendLine("^BXN,3,200");
            sb.AppendLine("^FD" + EscapeZpl(payload) + "^FS");

            // 텍스트 3줄 (사진 형식)
            sb.AppendLine("^FX");
            sb.AppendLine($"^FO150,30^A0N,20,20^FD{EscapeZpl(line1)}^FS");
            sb.AppendLine($"^FO150,50^A0N,20,20^FD{EscapeZpl(line2)}^FS");
            sb.AppendLine($"^FO150,70^A0N,20,20^FD{EscapeZpl(line3)}^FS");

            sb.AppendLine("^XZ");
            return sb.ToString();
        }

        private static int MmToDots(double mm, int dpi)
            => (int)Math.Round(mm * dpi / 25.4);

        private static string EscapeZpl(string s)
        {
            s ??= "";
            return s.Replace("^", "");
        }
    }
}
