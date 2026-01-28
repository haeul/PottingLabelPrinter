using PottingLabelPrinter.Modbus;

namespace PottingLabelPrinter.Models
{
    /// <summary>
    /// “포팅 완료 신호”가 어디에 있는지 정의하는 설정 묶음.
    /// - 현재 프로젝트 기준: FC=0x02(Read Discrete Inputs), IN 비트 판별
    /// </summary>
    public sealed class PottingDoneSpec
    {
        public byte[] Slaves { get; set; } = new byte[] { 1 };

        /// <summary>
        /// 포팅기 완료 신호는 FC=0x02(Discrete Inputs)로 읽는다.
        /// </summary>
        public FunctionCode Function { get; set; } = FunctionCode.ReadDiscreteInputs;

        /// <summary>
        /// 입력 시작 주소(0-base). (과장님 자료: 0x0000)
        /// </summary>
        public ushort Address { get; set; } = 0;

        /// <summary>
        /// 완료 신호가 들어있는 입력 비트 인덱스(0-based).
        /// - IN1 => 0
        /// - IN2 => 1 (현재 기준)
        /// </summary>
        public int BitIndex { get; set; } = 1;

        /// <summary>
        /// Active-Low 같은 케이스면 true로 반전해서 판단한다.
        /// </summary>
        public bool Invert { get; set; } = false;

        /// <summary>
        /// 폴링 주기(ms).
        /// </summary>
        public int IntervalMs { get; set; } = 100;
    }
}
