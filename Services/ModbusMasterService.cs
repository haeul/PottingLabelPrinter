using System;
using System.Text;
using PottingLabelPrinter.Modbus;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// Modbus 송수신을 책임지는 서비스.
    /// - 프레임 구성/CRC: ModbusRtu
    /// - 실제 전송: SerialModbusClient
    /// </summary>
    public class ModbusMasterService
    {
        private readonly SerialModbusClient _client;

        public ModbusMasterService(SerialModbusClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public class ModbusReadResult
        {
            public byte[] Request { get; set; } = Array.Empty<byte>();
            public byte[] Response { get; set; } = Array.Empty<byte>();
            public ushort[] Values { get; set; } = Array.Empty<ushort>();
            
            // FC=01/02용(비트 응답)
            public byte[] BitBytes { get; set; } = Array.Empty<byte>();
        }

        public ModbusReadResult ReadRegisters(byte slave, FunctionCode fc, ushort start, ushort count)
        {
            if (count == 0)
                throw new ArgumentOutOfRangeException(nameof(count), "count는 0보다 커야 합니다.");

            byte fcByte = (byte)fc;
            if (fcByte != 0x03 && fcByte != 0x04)
                throw new NotSupportedException("ReadRegisters는 FC=03,04만 지원합니다.");

            var req = ModbusRtu.BuildReadFrame(slave, fcByte, start, count);
            var resp = _client.SendAndReceive(req);

            if (resp == null || resp.Length == 0)
                throw new Exception("응답이 없습니다. (타임아웃/배선/포트 설정/Slave ID 확인)");

            if (resp.Length < 5)
                throw new Exception($"응답 프레임이 너무 짧습니다. (len={resp.Length}) RX={ToHex(resp)}");

            if (!HasValidCrc(resp))
                throw new Exception($"CRC 오류로 보이는 응답입니다. RX={ToHex(resp)}");

            if (resp[0] != slave)
                throw new Exception($"슬레이브 주소 불일치 (요청:{slave} / 응답:{resp[0]}) RX={ToHex(resp)}");

            // 예외 응답: FC에 0x80 OR
            if (resp[1] == (byte)(fcByte | 0x80))
                throw new Exception($"장치 예외 응답 (FC=0x{resp[1]:X2}, EX=0x{resp[2]:X2}) RX={ToHex(resp)}");

            // 정상 Read 응답 길이 검증: [Slave][FC][ByteCount][Data...][CRC(2)]
            int byteCount = resp[2];
            int expectedLen = 3 + byteCount + 2;
            if (resp.Length < expectedLen)
                throw new Exception($"응답 길이가 부족합니다. (expected>={expectedLen}, actual={resp.Length}) RX={ToHex(resp)}");

            var values = ModbusRtu.ParseReadResponse(resp);

            return new ModbusReadResult
            {
                Request = req,
                Response = resp,
                Values = values
            };
        }

        /// <summary>
        /// FC=02 (Read Discrete Inputs) 비트 읽기
        /// - start: 입력 시작 주소(0-base)
        /// - count: 읽을 입력 개수(bit 개수)
        /// </summary>
        public ModbusReadResult ReadDiscreteInputs(byte slave, ushort start, ushort count)
        {
            if (count == 0)
                throw new ArgumentOutOfRangeException(nameof(count), "count는 0보다 커야 합니다.");

            byte fcByte = 0x02;

            var req = ModbusRtu.BuildReadFrame(slave, fcByte, start, count);
            var resp = _client.SendAndReceive(req);

            ValidateCommon(slave, fcByte, resp);

            int byteCount = resp[2];
            int expectedLen = 3 + byteCount + 2;
            if (resp.Length < expectedLen)
                throw new Exception($"응답 길이가 부족합니다. (expected>={expectedLen}, actual={resp.Length}) RX={ToHex(resp)}");

            var bitBytes = ModbusRtu.ParseBitReadResponse(resp);

            return new ModbusReadResult
            {
                Request = req,
                Response = resp,
                BitBytes = bitBytes
            };
        }

        private static void ValidateCommon(byte slave, byte fcByte, byte[]? resp)
        {
            if (resp == null || resp.Length == 0)
                throw new Exception("응답이 없습니다. (타임아웃/배선/포트 설정/Slave ID 확인)");

            if (resp.Length < 5)
                throw new Exception($"응답 프레임이 너무 짧습니다. (len={resp.Length}) RX={ToHex(resp)}");

            if (!HasValidCrc(resp))
                throw new Exception($"CRC 오류로 보이는 응답입니다. RX={ToHex(resp)}");

            if (resp[0] != slave)
                throw new Exception($"슬레이브 주소 불일치 (요청:{slave} / 응답:{resp[0]}) RX={ToHex(resp)}");

            // 예외 응답: FC에 0x80 OR
            if (resp[1] == (byte)(fcByte | 0x80))
                throw new Exception($"장치 예외 응답 (FC=0x{resp[1]:X2}, EX=0x{resp[2]:X2}) RX={ToHex(resp)}");

            if (resp[1] != fcByte)
                throw new Exception($"FC 불일치 (요청:0x{fcByte:X2} / 응답:0x{resp[1]:X2}) RX={ToHex(resp)}");
        }

        private static bool HasValidCrc(byte[] frame)
        {
            if (frame == null || frame.Length < 3) return false;

            int len = frame.Length;
            ushort crc = ModbusRtu.Crc16(frame, len - 2);
            byte crcLo = (byte)(crc & 0xFF);
            byte crcHi = (byte)((crc >> 8) & 0xFF);

            return frame[len - 2] == crcLo && frame[len - 1] == crcHi;
        }

        private static string ToHex(byte[] data)
        {
            if (data == null || data.Length == 0) return "";
            var sb = new StringBuilder(data.Length * 3);
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(data[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
