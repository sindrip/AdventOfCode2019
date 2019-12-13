using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Days.IntCode;

namespace Days
{
    public class Day11 : IDay
    {
        private List<BigInteger> _instructions;
        public Day11()
        {
            _instructions = File.ReadAllText("input/day11.txt")
                .Split(',')
                .Select(x => BigInteger.Parse(x)).ToList();
        }
        public string Part1()
        {
            var painted = new HashSet<(int, int)>();
            var white = new HashSet<(int, int)>();
            var position = (0, 0);
            var direction = (0, 1);
            var vm = new Vm(_instructions);
            //vm.AddInput(1);
            do
            {
                var input = white.Contains(position) ? 1 : 0;
                vm.AddInput(input);
                vm.Run();
                var color = vm.Output[^2];
                if (color == 1)
                {
                    painted.Add(position);
                    white.Add(position);
                }
                else
                {
                    painted.Add(position);
                    white.Remove(position);
                }

                direction = GetDirection(direction, vm.Output[^1]);
                position = (position.Item1 + direction.Item1, position.Item2 + direction.Item2);

            } while (vm.Running);

            return painted.Count.ToString();
        }

        private (int, int) GetDirection((int, int) curr, BigInteger output)
        {
            return int.Parse(output.ToString()) switch
            {
                0 => (-curr.Item2, curr.Item1),
                1 => (curr.Item2, -curr.Item1),
            };
        }

        public string Part2()
        {
            var painted = new HashSet<(int, int)>();
            var white = new HashSet<(int, int)>();
            var position = (0, 0);
            var direction = (0, 1);
            var vm = new Vm(_instructions);
            //vm.AddInput(1);
            white.Add(position);
            do
            {
                var input = white.Contains(position) ? 1 : 0;
                vm.AddInput(input);
                vm.Run();
                var color = vm.Output[^2];
                if (color == 1)
                {
                    painted.Add(position);
                    white.Add(position);
                }
                else
                {
                    painted.Add(position);
                    white.Remove(position);
                }

                direction = GetDirection(direction, vm.Output[^1]);
                position = (position.Item1 + direction.Item1, position.Item2 + direction.Item2);

            } while (vm.Running);

            var minX = painted.Min(x => x.Item1);
            var minY = painted.Min(x => x.Item2);
            var maxX = painted.Max(x => x.Item1);
            var maxY = painted.Max(x => x.Item2);

            var sb = new StringBuilder(); 
            for (var y = maxY; y >= minY; y--)
            {
                sb.Append("\n");
                for (var x = minX; x <= maxX; x++)
                    sb.Append(white.Contains((x,y)) ? "█" : " ");
            }

            return sb.ToString();
        }
    }
}