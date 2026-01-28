using System;

namespace PottingLabelPrinter.Modbus
{
    /// <summary>
    /// Modbus Function Code 정의.
    /// (이번 프로젝트에서 최소한 Read만 쓰더라도 enum이 있으면 코드가 덜 헷갈린다.)
    /// </summary>
    public enum FunctionCode : byte
    {
        ReadDiscreteInputs = 0x02,
        ReadHoldingRegisters = 0x03,
        ReadInputRegisters = 0x04,
        WriteSingleRegister = 0x06,
        WriteMultipleRegisters = 0x10,
    }

    internal static class ModbusCrc16
    {
        /// <summary>
        /// Modbus RTU CRC-16 계산 (poly 0xA001, init 0xFFFF).
        /// </summary>
        public static ushort Compute(byte[] data, int length)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (length < 0 || length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            ushort crc = 0xFFFF;

            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];
                for (int b = 0; b < 8; b++)
                {
                    bool lsb = (crc & 0x0001) != 0;
                    crc >>= 1;
                    if (lsb)
                        crc ^= 0xA001;
                }
            }

            return crc;
        }

        /// <summary>
        /// CRC 값을 Modbus 규격에 맞게 (Lo, Hi) 순서로 버퍼에 기록.
        /// </summary>
        public static void AppendLittleEndian(byte[] buffer, int offset, ushort crc)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset + 1 >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            buffer[offset + 0] = (byte)(crc & 0xFF);        // CRC Lo
            buffer[offset + 1] = (byte)((crc >> 8) & 0xFF); // CRC Hi
        }
    }

    /// <summary>
    /// Modbus RTU 프레임 구성/파싱 + CRC 계산 담당.
    /// SerialPort, UI는 전혀 모른다.
    /// </summary>
    public static class ModbusRtu
    {
        public static ushort Crc16(byte[] data, int len)
            => ModbusCrc16.Compute(data, len);

        public static byte[] WithCrc(byte[] frameBody)
        {
            if (frameBody == null) throw new ArgumentNullException(nameof(frameBody));

            var crc = ModbusCrc16.Compute(frameBody, frameBody.Length);
            var arr = new byte[frameBody.Length + 2];

            Buffer.BlockCopy(frameBody, 0, arr, 0, frameBody.Length);
            ModbusCrc16.AppendLittleEndian(arr, frameBody.Length, crc);

            return arr;
        }

        /// <summary>
        /// FC=03,04 Read 프레임 생성.
        /// </summary>
        public static byte[] BuildReadFrame(byte slave, byte fc, ushort start, ushort count)
        {
            var raw = new byte[6];
            raw[0] = slave;
            raw[1] = fc;
            raw[2] = (byte)(start >> 8);
            raw[3] = (byte)(start & 0xFF);
            raw[4] = (byte)(count >> 8);
            raw[5] = (byte)(count & 0xFF);

            return WithCrc(raw);
        }

        /// <summary>
        /// Read 응답(FC=03,04)의 Data 영역을 ushort 배열로 파싱.
        /// </summary>
        public static ushort[] ParseReadResponse(byte[] resp)
        {
            if (resp == null || resp.Length < 5)
                return Array.Empty<ushort>();

            int bc = resp[2];    // Byte count
            int n = bc / 2;
            var arr = new ushort[n];

            for (int i = 0; i < n; i++)
            {
                int idx = 3 + i * 2;
                if (idx + 1 >= resp.Length) break;
                arr[i] = (ushort)((resp[idx] << 8) | resp[idx + 1]);
            }

            return arr;
        }

        /// <summary>
        /// Read 응답(FC=01,02)의 Data 영역을 byte 배열로 파싱.
        /// - bit packed: LSB가 첫 입력(IN1)
        /// </summary>
        public static byte[] ParseBitReadResponse(byte[] resp)
        {
            if (resp == null || resp.Length < 5)
                return Array.Empty<byte>();

            int bc = resp[2]; // Byte count
            var data = new byte[bc];
            if (3 + bc > resp.Length - 2) // CRC 2바이트 제외
                return Array.Empty<byte>();

            Buffer.BlockCopy(resp, 3, data, 0, bc);
            return data;
        }
    }
}
