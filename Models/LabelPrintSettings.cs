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

    public enum LabelElementKey
    {
        DataMatrix = 1,
        TrayNo = 2,
        Date = 3,
        Time = 4
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
        public LabelElementKey Key { get; set; }
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
                        Type = LabelElementType.DataMatrix,
                        Xmm = 10.01m,
                        Ymm = 3.75m,
                        Rotation = 0m,
                        FontSizeMm = 0.375m, // BXN,3 에 맞춤 (모듈 mm)
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "" // DM payload는 런타임에서 채움
                    },
                    new LabelElement
                    {
                        No = 2,
                        Type = LabelElementType.Text,
                        Xmm = 18.77m,
                        Ymm = 3.75m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m, // A0N,20,20 ≒ 2.5mm
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "Tray0001"
                    },
                    new LabelElement
                    {
                        No = 3,
                        Type = LabelElementType.Text,
                        Xmm = 18.77m,
                        Ymm = 6.26m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "2026-01-29"
                    },
                    new LabelElement
                    {
                        No = 4,
                        Type = LabelElementType.Text,
                        Xmm = 18.77m,
                        Ymm = 8.76m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "20:55:12"
                    }
                }
            };
        }
    }
}