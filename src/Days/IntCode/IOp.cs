using System.Collections.Generic;
using System.Numerics;

namespace Days.IntCode
{
    public interface IOp
    {
        OpCode Op { get; }
        IList<int> ParameterMode { get; }
        BigInteger OpWidth { get; }
    }
}