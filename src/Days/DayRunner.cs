using System;

namespace Days
{
    public static class DayRunner
    {
        public static void Run(IDay day)
        {
            var name = day.GetType().ToString();
            Console.WriteLine($"===================={name}====================");
            Console.WriteLine($"Part1: {day.Part1()}");
            Console.WriteLine($"Part2: {day.Part2()}");
            Console.WriteLine("==================================================");
        }
    }
}