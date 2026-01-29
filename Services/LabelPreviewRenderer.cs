using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class LabelPreviewRenderer
    {
        // 출력 보정과 동일하게 미리보기 반영
        private const double NudgeYmm = 0.6;

        public static void DrawPreview(Graphics graphics, Rectangle bounds, PrintSettingModel model, int dpi, double paddingPx)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // ---- 품질 옵션 (이게 “허접함” 체감 1순위) ----
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graphics.Clear(Color.White);

            double labelWmm = (double)model.Geometry.LabelWidthMm;
            double labelHmm = (double)model.Geometry.LabelHeightMm;

            double availableW = Math.Max(1, bounds.Width - paddingPx * 2);
            double availableH = Math.Max(1, bounds.Height - paddingPx * 2);

            double mm2px = Math.Min(availableW / labelWmm, availableH / labelHmm);
            float labelWidthPx = (float)(labelWmm * mm2px);
            float labelHeightPx = (float)(labelHmm * mm2px);

            float originX = bounds.Left + (float)paddingPx + (float)((availableW - labelWidthPx) / 2);
            float originY = bounds.Top + (float)paddingPx + (float)((availableH - labelHeightPx) / 2);

            var labelRect = new RectangleF(originX, originY, labelWidthPx, labelHeightPx);

            // ---- 라벨 배경/테두리(라운드) ----
            using (var bg = new SolidBrush(Color.White))
            using (var pen = new Pen(Color.Black, 1f))
            using (var path = CreateRoundedRectPath(labelRect, radiusPx: Math.Max(6f, (float)(Math.Min(labelWidthPx, labelHeightPx) * 0.03f))))
            {
                graphics.FillPath(bg, path);
                graphics.DrawPath(pen, path);
            }

            // ---- 좌표 변환(mm -> px), 출력과 동일하게 offset + 도트양자화 ----
            float ConvertMmToPreviewX(double mm)
            {
                double mmWithOffset = mm + (double)model.Geometry.OffsetXmm;
                int quantizedDots = MmToDotsInt(mmWithOffset, dpi);
                double quantizedMm = DotsToMm(quantizedDots, dpi);
                return originX + (float)(quantizedMm * mm2px);
            }

            float ConvertMmToPreviewY(double mm)
            {
                double mmWithOffset = mm + (double)model.Geometry.OffsetYmm + NudgeYmm;
                int quantizedDots = MmToDotsInt(mmWithOffset, dpi);
                double quantizedMm = DotsToMm(quantizedDots, dpi);
                return originY + (float)(quantizedMm * mm2px);
            }

            foreach (var element in model.Elements)
            {
                DrawElement(graphics, element, mm2px, ConvertMmToPreviewX, ConvertMmToPreviewY, dpi);
            }
        }

        private static void DrawElement(
            Graphics graphics,
            LabelElement element,
            double mm2px,
            Func<double, float> convertX,
            Func<double, float> convertY,
            int dpi)
        {
            float x = convertX((double)element.Xmm);
            float y = convertY((double)element.Ymm);

            // 변환이 누적되지 않게 Save/Restore
            var gstate = graphics.Save();
            try
            {
                // 기본: 좌상단 기준으로 배치한 뒤, “센터 기준 회전”이 되도록
                // (회전이 예쁘게 보이려면 중심 기준이 훨씬 낫다)
                if (element.Type == LabelElementType.DataMatrix)
                {
                    // DM 크기(px) 먼저 계산 -> 중심 회전
                    GetDataMatrixSizePx(element, mm2px, dpi, out float sidePx);

                    graphics.TranslateTransform(x + sidePx / 2f, y + sidePx / 2f);
                    graphics.RotateTransform((float)element.Rotation);
                    graphics.TranslateTransform(-sidePx / 2f, -sidePx / 2f);

                    DrawDataMatrix(graphics, element, mm2px, dpi, sidePx);
                }
                else
                {
                    // 텍스트는 측정해서 중심 회전
                    SizeF textSizePx = MeasureTextSizePx(graphics, element, mm2px, dpi, out Font fontForMeasure);
                    fontForMeasure.Dispose();

                    graphics.TranslateTransform(x + textSizePx.Width / 2f, y + textSizePx.Height / 2f);
                    graphics.RotateTransform((float)element.Rotation);
                    graphics.TranslateTransform(-textSizePx.Width / 2f, -textSizePx.Height / 2f);

                    DrawText(graphics, element, mm2px, dpi);
                }
            }
            finally
            {
                graphics.Restore(gstate);
            }
        }

        // -------------------- TEXT --------------------
        private static void DrawText(Graphics graphics, LabelElement element, double mm2px, int dpi)
        {
            // “출력과 동일한 도트 양자화” 기반으로 폰트 높이(mm) 계산
            // ScaleY는 폰트 높이에 반영하고, ScaleX/ScaleY는 Transform으로도 반영(이전 코드 느낌)
            double scaledFontMmY = (double)(element.FontSizeMm * element.ScaleY);
            int hDots = Math.Max(1, MmToDotsInt(scaledFontMmY, dpi));
            double hMmQ = DotsToMm(hDots, dpi);

            float fontPx = (float)(hMmQ * mm2px);
            float fontPt = Math.Max(1f, fontPx * 72f / graphics.DpiY);

            using (var font = new Font("Segoe UI", fontPt, FontStyle.Regular, GraphicsUnit.Point))
            using (var brush = new SolidBrush(Color.Black))
            {
                var st = graphics.Save();
                try
                {
                    // ScaleX/ScaleY를 실제 그래픽 변환에 적용 (이게 “이전처럼” 보이는 핵심)
                    float sx = (float)Math.Max(0.01, (double)element.ScaleX);
                    float sy = (float)Math.Max(0.01, (double)element.ScaleY);
                    if (Math.Abs(sx - 1f) > 1e-6 || Math.Abs(sy - 1f) > 1e-6)
                        graphics.ScaleTransform(sx, sy);

                    graphics.DrawString(element.Value ?? "", font, brush, new PointF(0, 0));
                }
                finally
                {
                    graphics.Restore(st);
                }
            }
        }

        private static SizeF MeasureTextSizePx(Graphics g, LabelElement element, double mm2px, int dpi, out Font measureFont)
        {
            double scaledFontMmY = (double)(element.FontSizeMm * element.ScaleY);
            int hDots = Math.Max(1, MmToDotsInt(scaledFontMmY, dpi));
            double hMmQ = DotsToMm(hDots, dpi);

            float fontPx = (float)(hMmQ * mm2px);
            float fontPt = Math.Max(1f, fontPx * 72f / g.DpiY);

            measureFont = new Font("Segoe UI", fontPt, FontStyle.Regular, GraphicsUnit.Point);

            // ScaleTransform까지 감안한 대략 크기(회전 중심용)
            var baseSize = g.MeasureString(element.Value ?? "", measureFont);
            float sx = (float)Math.Max(0.01, (double)element.ScaleX);
            float sy = (float)Math.Max(0.01, (double)element.ScaleY);
            return new SizeF(baseSize.Width * sx, baseSize.Height * sy);
        }

        // -------------------- DATAMATRIX --------------------
        private static void DrawDataMatrix(Graphics graphics, LabelElement element, double mm2px, int dpi, float sidePx)
        {
            using (var pen = new Pen(Color.Black, 1f))
            {
                graphics.DrawRectangle(pen, 0, 0, sidePx, sidePx);
            }

            // 중앙 “DM” 표식(이전처럼 보기 좋게)
            float textPx = Math.Max(6f, Math.Min(sidePx * 0.28f, 28f));
            float textPt = Math.Max(1f, textPx * 72f / graphics.DpiY);

            using (var font = new Font("Segoe UI", textPt, FontStyle.Bold, GraphicsUnit.Point))
            using (var brush = new SolidBrush(Color.Black))
            using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.DrawString("DM", font, brush, new RectangleF(0, 0, sidePx, sidePx), fmt);
            }
        }

        private static void GetDataMatrixSizePx(LabelElement element, double mm2px, int dpi, out float sidePx)
        {
            // module 크기(mm): 기존 로직 유지 + 도트 양자화
            double moduleMm = Math.Max(0.1, (double)(element.FontSizeMm * element.ScaleX));
            int moduleDots = Math.Max(1, MmToDotsInt(moduleMm, dpi));
            double moduleMmQ = DotsToMm(moduleDots, dpi);

            // 모듈 수: 12 고정 제거 -> 데이터 길이 기반 추정
            int modules = EstimateDmModulesFromData(element.Value ?? "");
            sidePx = (float)(moduleMmQ * modules * mm2px);
        }

        // DHSTesterXL에서 쓰던 “데이터 길이 기반 간이 추정” 그대로
        private static int EstimateDmModulesFromData(string s)
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

        // -------------------- DRAW UTILS --------------------
        private static GraphicsPath CreateRoundedRectPath(RectangleF bounds, float radiusPx)
        {
            var path = new GraphicsPath();
            float r = Math.Max(0f, radiusPx);
            float d = r * 2f;

            if (r <= 0.1f)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static int MmToDotsInt(double mm, int dpi)
            => (int)Math.Round(mm * dpi / 25.4);

        private static double DotsToMm(int dots, int dpi)
            => dots * 25.4 / dpi;
    }
}
