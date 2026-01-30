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
        public int StartNo { get; set; } = 1; // 시작 번호 (PrintSetting 창 테스트 출력 전용)
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

        // NEW: 요소별 표시/출력 여부 (Grid 체크박스 용)
        public bool ShowPreview { get; set; } = true;
        public bool ShowPrint { get; set; } = true;

        public decimal Xmm { get; set; }
        public decimal Ymm { get; set; }
        public decimal Rotation { get; set; } // 회전(도)
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
                        ShowPreview = true,
                        ShowPrint = true,
                        Xmm = 10.01m,
                        Ymm = 3.75m,
                        Rotation = 0m,
                        FontSizeMm = 7.0m, // DM 한 변(mm)
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "{PAYLOAD}"
                    },
                    new LabelElement
                    {
                        No = 2,
                        Type = LabelElementType.Text,
                        ShowPreview = true,
                        ShowPrint = true,
                        Xmm = 18.77m,
                        Ymm = 3.75m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "{TRAY}"
                    },
                    new LabelElement
                    {
                        No = 3,
                        Type = LabelElementType.Text,
                        ShowPreview = true,
                        ShowPrint = true,
                        Xmm = 18.77m,
                        Ymm = 6.26m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "{DATE}"
                    },
                    new LabelElement
                    {
                        No = 4,
                        Type = LabelElementType.Text,
                        ShowPreview = true,
                        ShowPrint = true,
                        Xmm = 18.77m,
                        Ymm = 8.76m,
                        Rotation = 0m,
                        FontSizeMm = 2.50m,
                        ScaleX = 1m,
                        ScaleY = 1m,
                        Value = "{TIME}"
                    }
                }
            };
        }
    }
}
