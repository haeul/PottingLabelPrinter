using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PottingLabelPrinter.Utils
{
    public sealed class TestDiagnostics
    {
        private static readonly Lazy<TestDiagnostics> _instance = new Lazy<TestDiagnostics>(() => new TestDiagnostics());
        public static TestDiagnostics Instance => _instance.Value;

        private readonly object _gate = new object();
        private readonly Dictionary<string, DateTime> _lastLogAt = new Dictionary<string, DateTime>();

        private long _pollingTickCount;
        private DateTime _lastPollAt = DateTime.MinValue;
        private byte? _lastRawValue;
        private bool? _lastDone;
        private DateTime _lastDoneRiseAt = DateTime.MinValue;
        private DateTime _lastAutoPrintStartAt = DateTime.MinValue;
        private DateTime _lastAutoPrintEndAt = DateTime.MinValue;
        private string _lastExceptionMessage = "";

        private TestDiagnostics()
        {
        }

        public void RecordPollingTick(DateTime now)
        {
            lock (_gate)
            {
                _pollingTickCount++;
                _lastPollAt = now;
            }
        }

        public void RecordModbusRead(byte raw, bool done, DateTime now)
        {
            lock (_gate)
            {
                _lastRawValue = raw;
                _lastDone = done;
                _lastPollAt = now;
            }
        }

        public void RecordDoneTransition(DateTime now, bool nowDone)
        {
            lock (_gate)
            {
                _lastDone = nowDone;
                if (nowDone)
                    _lastDoneRiseAt = now;
            }
        }

        public void RecordSyntheticDone(DateTime now)
        {
            lock (_gate)
            {
                _lastDone = true;
                _lastDoneRiseAt = now;
            }
        }

        public void RecordAutoPrintStart(DateTime now)
        {
            lock (_gate)
            {
                _lastAutoPrintStartAt = now;
            }
        }

        public void RecordAutoPrintEnd(DateTime now)
        {
            lock (_gate)
            {
                _lastAutoPrintEndAt = now;
            }
        }

        public void RecordException(string message, DateTime now)
        {
            lock (_gate)
            {
                _lastExceptionMessage = message ?? "";
                _lastPollAt = now;
            }
        }

        public void LogDoneTransition(byte slave, bool prev, bool current, DateTime now)
        {
            RecordDoneTransition(now, current);
            Debug.WriteLine($"[TEST][DONE] SID={slave} {prev} -> {current} at {now:HH:mm:ss.fff}");
        }

        public void LogAutoPrintStart(string source, DateTime now)
        {
            RecordAutoPrintStart(now);
            Debug.WriteLine($"[TEST][AUTO-PRINT] START source={source} at {now:HH:mm:ss.fff}");
        }

        public void LogAutoPrintEnd(string source, DateTime now)
        {
            RecordAutoPrintEnd(now);
            Debug.WriteLine($"[TEST][AUTO-PRINT] END   source={source} at {now:HH:mm:ss.fff}");
        }

        public void LogModbusException(byte slave, string message, DateTime now)
        {
            RecordException(message, now);

            if (!ShouldLog("modbus-error", TimeSpan.FromSeconds(2), now))
                return;

            Debug.WriteLine($"[TEST][MODBUS][ERR] SID={slave} {message} at {now:HH:mm:ss.fff}");
        }

        public string GetSnapshotText()
        {
            lock (_gate)
            {
                return string.Join(Environment.NewLine, new[]
                {
                    $"PollingTickCount : {_pollingTickCount}",
                    $"LastPollAt       : {FormatTime(_lastPollAt)}",
                    $"LastRawValue     : {( _lastRawValue.HasValue ? $"0x{_lastRawValue.Value:X2}" : "N/A")}",
                    $"LastDone         : {(_lastDone.HasValue ? _lastDone.Value.ToString() : "N/A")}",
                    $"LastDoneRiseAt   : {FormatTime(_lastDoneRiseAt)}",
                    $"LastAutoPrintStartAt : {FormatTime(_lastAutoPrintStartAt)}",
                    $"LastAutoPrintEndAt   : {FormatTime(_lastAutoPrintEndAt)}",
                    $"LastException    : {_lastExceptionMessage}"
                });
            }
        }

        private bool ShouldLog(string key, TimeSpan interval, DateTime now)
        {
            lock (_gate)
            {
                if (_lastLogAt.TryGetValue(key, out var lastAt))
                {
                    if (now - lastAt < interval)
                        return false;
                }

                _lastLogAt[key] = now;
                return true;
            }
        }

        private static string FormatTime(DateTime time)
        {
            return time == DateTime.MinValue ? "-" : time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
