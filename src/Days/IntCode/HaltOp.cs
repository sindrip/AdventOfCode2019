using System.Collections.Generic;

namespace Days.IntCode
{
    public class HaltOp : IOp
    {
        public OpCode Op { get; } = OpCode.Halt;
        public IList<int> ParameterMode { get; } = new List<int>();
        public int OpWidth { get; } = 1;

        public HaltOp()
        {
        }
    }
}