using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Days.Extensions;

namespace Days
{
    public class Day08: IDay
    {
        private List<int> _nums;
        private List<List<int>> _partitions;
        public Day08()
        {
            _nums = File.ReadAllText("input/day08.txt").ToCharArray().Select(x => int.Parse(x.ToString())).ToList();
            _partitions =_nums.Partition(25 * 6);
        }
        public string Part1()
        {
            var min = _partitions.Select(p => p.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()))
                .OrderBy(x => x[0]).First();

            return (min[1] * min[2]).ToString();
        }

        public string Part2()
        {
            // https://stackoverflow.com/questions/39484996/rotate-transposing-a-listliststring-using-linq-c-sharp
            List<List<int>> rotated = Enumerable.Range(0, _partitions.First().Count)
                .Select(i => _partitions.Select(l => l.ElementAtOrDefault(i)).ToList())
                .ToList();

            var colors = rotated.Select(GetColor).ToList();
            var sb = new StringBuilder();
            sb.Append("\n");
            for (var i = 0; i < 6; i++)
            {
                sb.Append(string.Join(null, colors.GetRange(i * 25, 25)) + "\n");
            }

            return sb.ToString();
        }

        private string GetColor(List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] != 2)
                    return l[i] == 0 ? "█" : " ";
            }

            return "█";
        }
        
    }
}