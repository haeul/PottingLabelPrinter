using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PottingLabelPrinter.Printer
{
    /// <summary>
    /// Winspool RAW 전송으로 ZPL을 프린터에 직접 전달한다.
    /// 드라이버 렌더링 없이 ZPL을 그대로 넣는 방식이라 Zebra에서 안정적이다.
    /// </summary>
    public static class RawPrinter
    {
        public static bool SendRawToPrinter(string printerName, string zpl)
        {
            if (string.IsNullOrWhiteSpace(printerName))
                throw new ArgumentException("printerName is empty.", nameof(printerName));

            if (string.IsNullOrWhiteSpace(zpl))
                throw new ArgumentException("zpl is empty.", nameof(zpl));

            IntPtr printerHandle;

            if (!OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
                return false;

            var docInfo = new DOCINFOA
            {
                pDocName = "ZPL Job",
                pDataType = "RAW"
            };

            if (!StartDocPrinter(printerHandle, 1, docInfo))
            {
                ClosePrinter(printerHandle);
                return false;
            }

            if (!StartPagePrinter(printerHandle))
            {
                EndDocPrinter(printerHandle);
                ClosePrinter(printerHandle);
                return false;
            }

            IntPtr unmanagedBuffer = IntPtr.Zero;
            try
            {
                // ZPL은 일반적으로 ASCII로 전송한다.
                byte[] bytes = Encoding.ASCII.GetBytes(zpl);
                unmanagedBuffer = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, unmanagedBuffer, bytes.Length);

                return WritePrinter(printerHandle, unmanagedBuffer, bytes.Length, out _);
            }
            finally
            {
                EndPagePrinter(printerHandle);
                EndDocPrinter(printerHandle);
                ClosePrinter(printerHandle);

                if (unmanagedBuffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(unmanagedBuffer);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName = "";
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile = "";
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType = "";
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true)]
        private static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, int level,
            [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", SetLastError = true)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
    }
}
