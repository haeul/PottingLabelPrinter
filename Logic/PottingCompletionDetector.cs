namespace PottingLabelPrinter.Logic
{
    /// <summary>
    /// 포팅 완료 판별 전용 클래스 (FC=0x02 Discrete Inputs 기준)
    /// - IN1=ON : 미완료 상태로 간주
    /// - IN2=ON : 완료 상태로 간주
    /// - "완료 트리거"는 IN2의 0->1 전이(상승엣지)로 판단
    /// </summary>
    public class PottingCompletionDetector
    {
        private bool _lastIn2 = false;

        /// <summary>
        /// 입력 비트 상태를 받아 "이번 사이클에서 완료 이벤트가 발생했는지" 판단.
        /// - in1On: IN1 상태 (참고/상태표시용)
        /// - in2On: IN2 상태 (완료 신호)
        /// </summary>
        public bool IsCompleted(bool in1On, bool in2On)
        {
            // 완료 이벤트는 IN2 상승엣지
            bool completed = (!_lastIn2 && in2On);
            _lastIn2 = in2On;
            return completed;
        }

        public void Reset()
        {
            _lastIn2 = false;
        }
    }
}
