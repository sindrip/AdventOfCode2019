using System.Collections.Generic;

namespace Days.IntCode
{
    public interface IOp
    {
        OpCode Op { get; }
        IList<int> ParameterMode { get; }
        int OpWidth { get; }
    }
}