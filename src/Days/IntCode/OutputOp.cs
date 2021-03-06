using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class OutputOp : IOp
    {
        
        public OpCode Op { get; } = OpCode.Output;
        public IList<int> ParameterMode { get; } = new List<int>() {0};
        public BigInteger OpWidth { get; } = 2;

        public OutputOp(IList<int> paramMode)
        {
            for (int i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}