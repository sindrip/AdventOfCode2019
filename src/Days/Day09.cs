using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Days.IntCode;

namespace Days
{
    public class Day09: IDay
    {
        private List<BigInteger> _instructions;
        public Day09()
        {
            _instructions = File.ReadAllText("input/day09.txt")
                .Split(',')
                .Select(x => BigInteger.Parse(x)).ToList();
        }
        
        public string Part1()
        {
            var vm = new Vm(_instructions);
            vm.AddInput(1);
            vm.Run();
            return string.Join(',', vm.Output);
            //return vm.MemAccess(0).ToString();
        }

        public string Part2()
        {
            var vm = new Vm(_instructions);
            vm.AddInput(2);
            vm.Run();
            return string.Join(',', vm.Output);
            //return vm.MemAccess(0).ToString();
        }
    }
}