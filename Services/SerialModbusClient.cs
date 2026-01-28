using System;
using System.IO.Ports;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// SerialPort를 통해 Modbus RTU 요청을 보내고 응답을 읽어오는 전담 클래스.
    /// - 포트는 외부에서 Open/Close 하고,
    /// - 이 클래스는 "열려 있는 포트"를 사용하기만 한다.
    /// </summary>
    public class SerialModbusClient
    {
        private readonly SerialPort _sp;
        private readonly object _ioLock = new object();

        public SerialModbusClient(SerialPort sp)
        {
            _sp = sp ?? throw new ArgumentNullException(nameof(sp));
        }

        /// <summary>
        /// Modbus RTU 프레임(TX)을 쓰고, 응답(RX)을 읽어서 그대로 반환한다.
        /// CRC 검증/파싱은 상위(ModbusMasterService, ModbusRtu)에서 처리한다.
        /// </summary>
        public byte[] SendAndReceive(byte[] request)
        {
            if (request == null || request.Length == 0)
                throw new ArgumentException("요청 프레임이 비어 있습니다.", nameof(request));

            if (!_sp.IsOpen)
                throw new InvalidOperationException("SerialPort가 열려 있지 않습니다.");

            lock (_ioLock)
            {
                _sp.DiscardInBuffer();
                _sp.Write(request, 0, request.Length);

                // 장비 응답은 분할되어 들어올 수 있어서,
                // 전체 타임아웃 범위 내에서 수신을 누적하고
                // 일정 시간(quiet) 동안 추가 바이트가 없으면 종료한다.
                var buf = new byte[2048];
                int read = 0;

                int overallTimeoutMs = _sp.ReadTimeout > 0 ? _sp.ReadTimeout : 500;
                const int quietTimeoutMs = 30;
                const int pollSleepMs = 5;

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var quietSw = System.Diagnostics.Stopwatch.StartNew();

                while (sw.ElapsedMilliseconds < overallTimeoutMs)
                {
                    int available = _sp.BytesToRead;

                    if (available > 0)
                    {
                        if (read + available > buf.Length)
                        {
                            int newLen = Math.Max(buf.Length * 2, read + available);
                            Array.Resize(ref buf, newLen);
                        }

                        int toRead = Math.Min(available, buf.Length - read);
                        int n = _sp.Read(buf, read, toRead);
                        if (n > 0)
                        {
                            read += n;
                            quietSw.Restart();
                        }
                    }
                    else
                    {
                        if (read == 0)
                        {
                            System.Threading.Thread.Sleep(pollSleepMs);
                            continue;
                        }

                        if (quietSw.ElapsedMilliseconds >= quietTimeoutMs)
                            break;

                        System.Threading.Thread.Sleep(pollSleepMs);
                    }
                }

                var resp = new byte[read];
                Buffer.BlockCopy(buf, 0, resp, 0, read);
                return resp;
            }
        }
    }
}
