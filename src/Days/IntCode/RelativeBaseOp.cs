using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public class RelativeBaseOp: IOp
    {
        
        public OpCode Op { get; } = OpCode.RelativeBase;
        public IList<int> ParameterMode { get; } = new List<int>() {0};
        public BigInteger OpWidth { get; } = 2;

        public RelativeBaseOp(IList<int> paramMode)
        {
            for (int i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}