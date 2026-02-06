using System;
using System.Windows.Forms;
using System.Collections.Generic;
using PottingLabelPrinter.Models;
using PottingLabelPrinter.Utils;
using PottingLabelPrinter.Modbus;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// Timer 기반 Modbus 폴링 서비스 (FC=0x02 Discrete Inputs).
    /// - IN1 ON: 미완료 상태(표시용)
    /// - IN2 ON: 완료 상태(트리거)
    /// - IN2가 0→1로 바뀌는 순간만 이벤트 발생
    /// </summary>
    public sealed class ModbusPollingService : IDisposable
    {
        private readonly ModbusSession _session;
        private readonly PottingDoneSpec _spec;
        private readonly EdgeLatch _latch = new EdgeLatch();
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        private bool _ticking;
        private readonly Dictionary<byte, bool> _lastDoneBySlave = new Dictionary<byte, bool>();

        public event EventHandler<PottingDoneDetectedEventArgs>? PottingDoneDetected;

        // (선택) 디버그/테스트용 상태 노출
        public byte LastInputsByte { get; private set; }
        public bool LastIn1 { get; private set; }
        public bool LastIn2 { get; private set; }

        public ModbusPollingService(ModbusSession session, PottingDoneSpec spec)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _spec = spec ?? throw new ArgumentNullException(nameof(spec));

            _timer.Interval = Math.Max(50, _spec.IntervalMs);
            _timer.Tick += Timer_Tick;
        }

        public bool IsRunning => _timer.Enabled;

        public void Start()
        {
            _timer.Interval = Math.Max(50, _spec.IntervalMs);
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        public void ResetLatch()
        {
            _latch.Clear();
        }

        public string LastCommStatus { get; private set; } = "INIT";
        public string LastCommError { get; private set; } = "";
        public DateTime LastCommAt { get; private set; } = DateTime.MinValue;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_ticking) return;
            _ticking = true;

            try
            {
                // ===== TEST/DIAGNOSTICS BEGIN =====
                var now = DateTime.Now;
                TestDiagnostics.Instance.RecordPollingTick(now);
                // ===== TEST/DIAGNOSTICS END =====

                if (!_session.IsOpen)
                {
                    LastCommStatus = "PORT CLOSE";
                    LastCommAt = DateTime.Now;
                    return;
                }

                // 현재 요구사항은 FC=0x02 고정
                if (_spec.Function != FunctionCode.ReadDiscreteInputs)
                    return;

                foreach (var slave in _spec.Slaves)
                {
                    // IN2(bit1)까지 필요 → 최소 2bit 읽기
                    ushort count = 2;

                    if (!_session.TryReadDiscreteInputs(slave, _spec.Address, count, out var bytes, out var err))
                    {
                        LastCommStatus = $"FAIL (SID={slave})";
                        LastCommError = err ?? "";
                        LastCommAt = DateTime.Now;
                        // ===== TEST/DIAGNOSTICS BEGIN =====
                        TestDiagnostics.Instance.LogModbusException(slave, LastCommError, DateTime.Now);
                        // ===== TEST/DIAGNOSTICS END =====
                        continue;
                    }

                    LastCommStatus = $"OK (SID={slave})";
                    LastCommError = "";
                    LastCommAt = DateTime.Now;


                    if (bytes.Length < 1)
                        continue;

                    byte b0 = bytes[0];
                    LastInputsByte = b0;

                    // LSB 기준: bit0=IN1, bit1=IN2
                    bool in1 = ((b0 >> 0) & 0x1) == 1;
                    bool in2 = ((b0 >> 1) & 0x1) == 1;

                    // Active-Low 같은 케이스면 반전
                    if (_spec.Invert)
                    {
                        in1 = !in1;
                        in2 = !in2;
                    }

                    LastIn1 = in1;
                    LastIn2 = in2;

                    // 완료 트리거는 IN2
                    bool nowDone = in2;

                    // ===== TEST/DIAGNOSTICS BEGIN =====
                    TestDiagnostics.Instance.RecordModbusRead(b0, nowDone, DateTime.Now);
                    if (_lastDoneBySlave.TryGetValue(slave, out var prevDone))
                    {
                        if (prevDone != nowDone)
                            TestDiagnostics.Instance.LogDoneTransition(slave, prevDone, nowDone, DateTime.Now);
                    }
                    else
                    {
                        TestDiagnostics.Instance.RecordDoneTransition(DateTime.Now, nowDone);
                    }

                    _lastDoneBySlave[slave] = nowDone;
                    // ===== TEST/DIAGNOSTICS END =====

                    if (_latch.Rise(slave, nowDone))
                    {
                        PottingDoneDetected?.Invoke(this, new PottingDoneDetectedEventArgs(slave, DateTime.Now));
                    }
                }
            }
            finally
            {
                _ticking = false;
            }
        }

        public void Dispose()
        {
            Stop();
            _timer.Tick -= Timer_Tick;
            _timer.Dispose();
        }
    }

    public sealed class PottingDoneDetectedEventArgs : EventArgs
    {
        public byte Slave { get; }
        public DateTime DetectedAt { get; }

        public PottingDoneDetectedEventArgs(byte slave, DateTime detectedAt)
        {
            Slave = slave;
            DetectedAt = detectedAt;
        }
    }
}
