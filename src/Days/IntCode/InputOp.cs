using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class InputOp : IOp
    {
        public OpCode Op { get; } = OpCode.Input;
        public IList<int> ParameterMode { get; } = new List<int>() {1};
        public BigInteger OpWidth { get; } = 2;

        public InputOp(IList<int> paramMode)
        {
            for (var i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}