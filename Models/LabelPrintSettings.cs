using System.Collections.Generic;

namespace PottingLabelPrinter.Models
{
    public enum LabelElementType
    {
        Text,
        DataMatrix
    }

    public enum LabelPrintDirection
    {
        Normal,
        Rotate90,
        Rotate180,
        Rotate270
    }

    public class PrintRuntimeSettings
    {
        public int Darkness { get; set; } = 10;
        public int Speed { get; set; } = 1;
        public int Quantity { get; set; } = 1;
        public int StartNo { get; set; } = 1; // 시작 번호
    }

    public class LabelGeometrySettings
    {
        public decimal LabelWidthMm { get; set; } = 44m;
        public decimal LabelHeightMm { get; set; } = 14m;
        public decimal GapMm { get; set; } = 3m; // 라벨 간격
        public decimal OffsetXmm { get; set; }
        public decimal OffsetYmm { get; set; }
    }

    public class LabelElement
    {
        public int No { get; set; }
        public LabelElementType Type { get; set; } = LabelElementType.Text;
        public decimal Xmm { get; set; }
        public decimal Ymm { get; set; }
        public decimal Rotation { get; set; } // NEW: 회전(도)
        public decimal FontSizeMm { get; set; } = 2.6m;
        public decimal ScaleX { get; set; } = 1m;
        public decimal ScaleY { get; set; } = 1m;
        public string Value { get; set; } = "";
    }

    public class PrintSettingModel
    {
        public PrintRuntimeSettings Print { get; set; } = new();
        public LabelGeometrySettings Geometry { get; set; } = new();
        public LabelPrintDirection PrintDirection { get; set; } = LabelPrintDirection.Normal;
        public List<LabelElement> Elements { get; set; } = new();

        public static PrintSettingModel CreateDefault()
        {
            return new PrintSettingModel
            {
                Elements = new List<LabelElement>
                {
                    new LabelElement
                    {
                        No = 1,
                        Type = LabelElementType.Text,
                        Xmm = 8m,
                        Ymm = 4m,
                        Rotation = 0m,
                        FontSizeMm = 2.6m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "TEXT"
                    },
                    new LabelElement
                    {
                        No = 2,
                        Type = LabelElementType.DataMatrix,
                        Xmm = 30m,
                        Ymm = 2m,
                        Rotation = 0m,
                        FontSizeMm = 0.6m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "DM"
                    }
                }
            };
        }
    }
}