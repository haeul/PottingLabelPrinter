using System;

namespace PottingLabelPrinter.Services
{
    /// <summary>
    /// 운전(장비 신호 기반) 자동 출력 전용 "다음 출력 번호" 상태.
    /// - 수동 출력(Print Setting)과 완전히 분리된 상태를 유지한다.
    /// - SetCurrentNo(n): 다음 출력 번호를 n으로 세팅
    /// - GetCurrentNo(): 현재 "다음 출력 번호"를 조회 (저장 포함)
    /// </summary>
    public static class AutoLabelSequenceState
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
    }
}
