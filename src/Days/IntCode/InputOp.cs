using System.Collections.Generic;

namespace Days.IntCode
{
    public class InputOp : IOp
    {
        public OpCode Op { get; } = OpCode.Input;
        public IList<int> ParameterMode { get; } = new List<int>() {1};
        public int OpWidth { get; } = 2;

        public InputOp()
        {
        }
    }
}