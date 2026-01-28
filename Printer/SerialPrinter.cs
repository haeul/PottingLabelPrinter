using System;
using System.IO.Ports;
using System.Text;

namespace PottingLabelPrinter.Printer
{
    /// <summary>
    /// Zebra ZD421 전용 연결 유지형 시리얼 프린터.
    /// - RS-232 / USB-Serial 공용
    /// - 설정은 ZD421 권장값으로 고정
    /// </summary>
    public sealed class SerialPrinter : IDisposable
    {
        private SerialPort? _port;

        public bool IsOpen => _port != null && _port.IsOpen;
        public string? PortName => _port?.PortName;

        /// <summary>
        /// ZD421 시리얼 포트 연결
        /// </summary>
        public void Connect(string comPort)
        {
            if (string.IsNullOrWhiteSpace(comPort))
                throw new ArgumentException("COM 포트가 비어 있습니다.", nameof(comPort));

            Disconnect();

            _port = new SerialPort
            {
                PortName = comPort,

                // ===== Zebra ZD421 고정 설정 =====
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                Encoding = Encoding.ASCII,

                WriteTimeout = 1000,
                ReadTimeout = 1000,
                DtrEnable = false,
                RtsEnable = false
            };

            _port.Open();
        }

        /// <summary>
        /// ZPL 출력 (연결 유지 상태에서 Write만 수행)
        /// </summary>
        public bool Print(string zpl)
        {
            if (!IsOpen)
                throw new InvalidOperationException("시리얼 프린터가 연결되어 있지 않습니다.");

            if (string.IsNullOrWhiteSpace(zpl))
                return false;

            try
            {
                _port!.Write(zpl);
                _port.BaseStream.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_port != null)
                {
                    if (_port.IsOpen)
                        _port.Close();

                    _port.Dispose();
                    _port = null;
                }
            }
            catch
            {
                // ignore
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
