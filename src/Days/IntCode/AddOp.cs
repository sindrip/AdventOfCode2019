using System.Collections.Generic;

namespace Days.IntCode
{
    public class AddOp : IOp
    {
        public OpCode Op { get; } = OpCode.Add;
        public IList<int> ParameterMode { get; } = new List<int>() {0, 0, 1};
        public int OpWidth { get; } = 4;

        public AddOp(IList<int> paramMode)
        {
            for (int i = 0; i < paramMode.Count; i++)
                ParameterMode[i] = paramMode[i];
        }
    }
}