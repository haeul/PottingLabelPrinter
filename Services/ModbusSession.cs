using System;
using System.IO.Ports;
using PottingLabelPrinter.Modbus;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// COM Board 포트 연결과 Modbus Read 호출을 묶어주는 세션.
    /// FormMain은 여기만 알면 된다.
    /// </summary>
    public sealed class ModbusSession : IDisposable
    {
        private SerialPort? _sp;
        private SerialModbusClient? _client;
        private ModbusMasterService? _master;

        public bool IsOpen => _sp != null && _sp.IsOpen;
        public string? LastError { get; private set; }

        public void Connect(string portName, int baudRate)
        {
            if (string.IsNullOrWhiteSpace(portName))
                throw new ArgumentException("포트명이 비어 있습니다.", nameof(portName));

            Disconnect();
            LastError = null;

            try
            {
                _sp = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,
                    ReadTimeout = 500,
                    WriteTimeout = 500
                };

                _sp.Open();

                _client = new SerialModbusClient(_sp);
                _master = new ModbusMasterService(_client);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Disconnect();
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                _master = null;
                _client = null;

                if (_sp != null)
                {
                    if (_sp.IsOpen) _sp.Close();
                    _sp.Dispose();
                    _sp = null;
                }
            }
            catch
            {
                // 닫기 실패는 치명적이지 않으니 조용히 정리한다.
            }
        }

        public bool TryReadDiscreteInputs(byte slave, ushort start, ushort count, out byte[] bytes, out string? error)
        {
            bytes = Array.Empty<byte>();
            error = null;

            if (!IsOpen || _master == null)
            {
                error = "보드 포트가 열려있지 않습니다.";
                return false;
            }

            try
            {
                var rr = _master.ReadDiscreteInputs(slave, start, count);
                bytes = rr.BitBytes ?? Array.Empty<byte>();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }


        public bool TryReadRegisters(byte slave, FunctionCode fc, ushort start, ushort count, out ushort[] values, out string? error)
        {
            values = Array.Empty<ushort>();
            error = null;

            if (!IsOpen || _master == null)
            {
                error = "보드 포트가 열려있지 않습니다.";
                return false;
            }

            try
            {
                var rr = _master.ReadRegisters(slave, fc, start, count);
                values = rr.Values ?? Array.Empty<ushort>();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
