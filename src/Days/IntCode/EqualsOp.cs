using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class EqualsOp : IOp
    {
        public OpCode Op { get; } = OpCode.Equals;
        public IList<int> ParameterMode { get; } = new List<int>() {0, 0, 1};
        public BigInteger OpWidth { get; } = 4;
        
        public EqualsOp(IList<int> paramMode)
        {
            for (var i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}