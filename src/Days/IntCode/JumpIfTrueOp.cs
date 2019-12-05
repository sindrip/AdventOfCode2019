using System.Collections.Generic;

namespace Days.IntCode
{
    public class JumpIfTrueOp : IOp
    {
        public OpCode Op { get; } = OpCode.JumpIfTrue;
        public IList<int> ParameterMode { get; } = new List<int>() {0, 0};
        public int OpWidth { get; } = 3;
        
        public JumpIfTrueOp(IList<int> paramMode)
        {
            for (var i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}