using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class JumpIfFalseOp : IOp
    {
        public OpCode Op { get; } = OpCode.JumpIfFalse;
        public IList<int> ParameterMode { get; } = new List<int>() {0, 0};
        public BigInteger OpWidth { get; } = 3;
        
        public JumpIfFalseOp(IList<int> paramMode)
        {
            for (var i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}