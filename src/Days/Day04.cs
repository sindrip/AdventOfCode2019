using System;
using System.Collections.Generic;
using System.Linq;

namespace Days
{
    public class Day04 : IDay
    {
        public string Part1()
        {
            var result = CountPaths(0, new int[10], 0, "", false);
            return result.ToString();
        }

        public string Part2()
        {
            var result = CountPaths(0, new int[10], 0, "", true);
            return result.ToString();
        }

        // Note how this works because of the monotonically increasing constraint of the digits of the number
        // A number cannot contain the same digit twice without them occurring in sequence
        private int CountPaths(int lastDigit, int[] counter, int noDigits, string currentNumber, bool part2)
        {
            if (noDigits == 6)
            {
                var number = int.Parse(currentNumber);
                if (number >= 356261 && number <= 846303)
                {
                    List<int> groups;
                    if (part2)
                        groups = counter.Where(x => x == 2).ToList();
                    else
                        groups = counter.Where(x => x >= 2).ToList();
                    if (groups.Count() > 0)
                        return 1;
                }
                return 0;
            }

            var p = 0;
            for (int digit = lastDigit; digit < 10; digit++)
            {
                var newCounter = new int[10];
                counter.CopyTo(newCounter, 0);
                newCounter[digit] += 1;
                var newNumber = currentNumber + digit;
                p += CountPaths(digit, newCounter, noDigits + 1, newNumber, part2);
            }

            return p;
        }
    }
}