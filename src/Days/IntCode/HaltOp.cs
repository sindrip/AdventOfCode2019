using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class HaltOp : IOp
    {
        public OpCode Op { get; } = OpCode.Halt;
        public IList<int> ParameterMode { get; } = new List<int>();
        public BigInteger OpWidth { get; } = 1;

        public HaltOp()
        {
        }
    }
}