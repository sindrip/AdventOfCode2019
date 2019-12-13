using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Days.Extensions;
using Days.IntCode;

namespace Days
{
    public class Day13: IDay
    {
        private List<BigInteger> _instructions;
        public Day13()
        {
            _instructions = File.ReadAllText("input/day13.txt")
                .Split(',')
                .Select(x => BigInteger.Parse(x)).ToList();
        }
        
        public string Part1()
        {
            var vm = new Vm(_instructions);
            vm.Run();

            var partitions = vm.Output.Partition(3);
            var tiles = new Dictionary<(BigInteger, BigInteger), BigInteger>();
            foreach (var p in partitions)
            {
                var key = (p[0], p[1]);
                var val = p[2];
                if (tiles.ContainsKey(key))
                    tiles[key] = val;
                else
                    tiles.Add(key, val);
            }

            return tiles.Where(x => x.Value == 2).Count().ToString();
        }

        public string Part2()
        {
            var vm = new Vm(_instructions);
            vm.MemSet(0, 2);
            // Start the field
            vm.Run();
            var partitions = vm.Output.Partition(3);
            var tiles = new Dictionary<(BigInteger, BigInteger), BigInteger>();

            var score = 0;
            foreach (var p in partitions)
            {
                var key = (p[0], p[1]);
                var val = p[2];

                if (key == (-1, 0))
                {
                    score = int.Parse(val.ToString());
                    continue;
                }
                
                if (tiles.ContainsKey(key))
                    tiles[key] = val;
                else
                    tiles.Add(key, val);
            }
            vm.DeleteOutput();
            
            do
            {
                partitions = vm.Output.Partition(3);
                foreach (var p in partitions)
                {
                    var key = (p[0], p[1]);
                    var val = p[2];

                    if (key == (-1, 0))
                    {
                        score = int.Parse(val.ToString());
                        continue;
                    }
                
                    if (tiles.ContainsKey(key))
                        tiles[key] = val;
                    else
                        tiles.Add(key, val);
                }
                vm.DeleteOutput();
                
                var ball = tiles.Where(x => x.Value == 4).First().Key;
                var paddle = tiles.Where(x => x.Value == 3).First().Key;
                var dir = ball.Item1.CompareTo(paddle.Item1);
                
                vm.AddInput(dir);
                vm.Run();
            } while (vm.Running);

            return vm.LastOutput().ToString();
        }
    }
}