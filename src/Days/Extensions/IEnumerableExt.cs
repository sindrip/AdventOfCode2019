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

        public static List<List<T>> Partition<T>(this List<T> list, int size)
        {
            var ret = new List<List<T>>();
            var count = list.Count();
            var partitions = count / size;
            for (int i = 0; i < partitions; i++)
            {
                var p = new List<T>();
                for (int j = 0; j < size; j++)
                {
                    p.Add(list[i * size + j]);
                }

                ret.Add(p);
            }

            return ret;
        }
        
        

    }
}