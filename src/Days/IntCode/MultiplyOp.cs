using System.Collections.Generic;

namespace Days.IntCode
{
    public class MultiplyOp : IOp
    {
        public OpCode Op { get; } = OpCode.Multiply;
        public IList<int> ParameterMode { get; } = new List<int>() {0, 0, 1};
        public int OpWidth { get; } = 4;
        
        public MultiplyOp(IList<int> paramMode)
        {
            for (var i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}