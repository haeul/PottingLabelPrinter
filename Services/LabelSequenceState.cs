using System;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// 운전(장비 신호 기반) 출력 시 사용할 "다음 출력 번호" 상태.
    /// - SetCurrentNo(n): 다음 출력 번호를 n으로 세팅
    /// - GetAndIncrement(): 현재 번호를 반환하고 다음 번호로 +1 (즉시 저장)
    /// </summary>
    public static class LabelSequenceState
    {
        private static readonly object _lock = new object();
        private static int _currentNo = -1;
        private static bool _loaded;

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            _currentNo = LabelSequenceStorage.LoadCurrentNo();
            _loaded = true;
        }

        public static int GetCurrentNo()
        {
            lock (_lock)
            {
                EnsureLoaded();
                return _currentNo;
            }
        }

        public static void SetCurrentNo(int nextNo)
        {
            lock (_lock)
            {
                EnsureLoaded();
                _currentNo = Math.Max(1, nextNo);
                LabelSequenceStorage.SaveCurrentNo(_currentNo);
            }
        }

        public static int GetAndIncrement()
        {
            lock (_lock)
            {
                EnsureLoaded();
                int now = Math.Max(1, _currentNo);

                // 다음 번호로 증가 후 저장
                _currentNo = now + 1;
                LabelSequenceStorage.SaveCurrentNo(_currentNo);

                return now;
            }
        }
    }
}
