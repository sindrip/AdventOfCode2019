using System.Collections.Generic;
using System.Linq;

namespace Days.Extensions
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Singleton<T>(this T element)
        {
            return Enumerable.Repeat<T>(element, 1);
        }
    }
}