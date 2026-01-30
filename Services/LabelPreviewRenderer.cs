// LabelPreviewRenderer.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using PottingLabelPrinter.Models;

namespace PottingLabelPrinter.Services
{
    public static class LabelPreviewRenderer
    {
        private const double NudgeYmm = 0.6;

        public static void DrawPreview(Graphics graphics, Rectangle bounds, PrintSettingModel model, int dpi, double paddingPx)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

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

            using (var bg = new SolidBrush(Color.White))
            using (var pen = new Pen(Color.Black, 1f))
            using (var path = CreateRoundedRectPath(labelRect, radiusPx: Math.Max(6f, (float)(Math.Min(labelWidthPx, labelHeightPx) * 0.03f))))
            {
                graphics.FillPath(bg, path);
                graphics.DrawPath(pen, path);
            }

            float ConvertMmToPreviewX(double mm)
            {
                double mmWithOffset = mm + (double)model.Geometry.OffsetXmm;
                int quantizedDots = LabelLayoutMath.MmToDotsInt(mmWithOffset, dpi);
                double quantizedMm = LabelLayoutMath.DotsToMm(quantizedDots, dpi);
                return originX + (float)(quantizedMm * mm2px);
            }

            float ConvertMmToPreviewY(double mm)
            {
                double mmWithOffset = mm + (double)model.Geometry.OffsetYmm + NudgeYmm;
                int quantizedDots = LabelLayoutMath.MmToDotsInt(mmWithOffset, dpi);
                double quantizedMm = LabelLayoutMath.DotsToMm(quantizedDots, dpi);
                return originY + (float)(quantizedMm * mm2px);
            }

            foreach (var element in model.Elements)
            {
                // NEW: 미리보기 체크 해제면 그리지 않음
                if (!element.ShowPreview)
                    continue;

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

            var gstate = graphics.Save();
            try
            {
                if (element.Type == LabelElementType.DataMatrix)
                {
                    // SSOT 적용: FontSizeMm = 목표 한 변(mm), Scale 무시
                    GetDataMatrixSizePx(element, mm2px, dpi, out float sidePx);

                    graphics.TranslateTransform(x + sidePx / 2f, y + sidePx / 2f);
                    graphics.RotateTransform((float)element.Rotation);
                    graphics.TranslateTransform(-sidePx / 2f, -sidePx / 2f);

                    DrawDataMatrix(graphics, element, mm2px, dpi, sidePx);
                }
                else
                {
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
            double scaledFontMmY = (double)(element.FontSizeMm * element.ScaleY);
            int hDots = Math.Max(1, LabelLayoutMath.MmToDotsInt(scaledFontMmY, dpi));
            double hMmQ = LabelLayoutMath.DotsToMm(hDots, dpi);

            float fontPx = (float)(hMmQ * mm2px);
            float fontPt = Math.Max(1f, fontPx * 72f / graphics.DpiY);

            using (var font = new Font("Segoe UI", fontPt, FontStyle.Regular, GraphicsUnit.Point))
            using (var brush = new SolidBrush(Color.Black))
            {
                var st = graphics.Save();
                try
                {
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
            int hDots = Math.Max(1, LabelLayoutMath.MmToDotsInt(scaledFontMmY, dpi));
            double hMmQ = LabelLayoutMath.DotsToMm(hDots, dpi);

            float fontPx = (float)(hMmQ * mm2px);
            float fontPt = Math.Max(1f, fontPx * 72f / g.DpiY);

            measureFont = new Font("Segoe UI", fontPt, FontStyle.Regular, GraphicsUnit.Point);

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
            // SSOT: FontSizeMm = 목표 한 변(mm), ScaleX/ScaleY 무시
            var dm = LabelLayoutMath.CalcDataMatrixDots(element, dpi);

            // 프리뷰도 "실제 출력 도트 양자화 결과(mm)"로 맞춤
            sidePx = (float)(dm.ActualSideMm * mm2px);
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
                    => LabelLayoutMath.MmToDotsInt(mm, dpi);

        private static double DotsToMm(int dots, int dpi)
            => LabelLayoutMath.DotsToMm(dots, dpi);
    }
}
