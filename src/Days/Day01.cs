using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Days
{
    public class Day01 : IDay
    {
        private IEnumerable<int> _nums;
        
        public Day01()
        {
            _nums = File.ReadLines("input/day01.txt").Select(l => int.Parse(l));
        }

        public string Part1() => _nums.Select(x => (x / 3) - 2).Sum().ToString();

        public string Part2() => _nums.Select(Part2Helper).Sum().ToString();
        
        private int Part2Helper(int mass)
        {
            var f = (mass / 3) - 2;
            if (f <= 0)
                return 0;

            return f + Part2Helper(f);
        }
    }
}