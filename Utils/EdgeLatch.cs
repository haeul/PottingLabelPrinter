using System.Collections.Generic;

namespace PottingLabelPrinter.Utils
{
    /// <summary>
    /// Slave별 이전 상태를 기억해서 0→1(상승엣지)만 통과시키는 도우미.
    /// 중복 출력 방지에 사용한다.
    /// </summary>
    public sealed class EdgeLatch
    {
        private readonly Dictionary<byte, bool> _prev = new Dictionary<byte, bool>();

        public bool Rise(byte slave, bool now)
        {
            bool prev = _prev.TryGetValue(slave, out var v) && v;
            _prev[slave] = now;

            return !prev && now;
        }

        public void Clear()
        {
            _prev.Clear();
        }
    }
}
