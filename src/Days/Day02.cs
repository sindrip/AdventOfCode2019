using System.Collections.Generic;
using System.IO;
using System.Linq;
using Days.IntCode;

namespace Days
{
    public class Day02 : IDay
    {

        private List<int> _nums;
        private Vm vm;
        public Day02()
        {
            _nums = File.ReadAllText("input/day02.txt")
                .Split(',')
                .Select(x => int.Parse(x)).ToList();
            vm = new Vm(_nums);
        }
        public string Part1()
        {
            vm.Run();
            return vm.MemAccess(0).ToString();
        }

        public string Part2()
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    vm.Reset();
                    vm.MemSet(1, noun);
                    vm.MemSet(2, verb);
                    vm.Run();
                    var result = vm.MemAccess(0);
                    if (result == 19690720)
                    {
                        var answer = noun * 100 + verb;
                        return answer.ToString();
                    }
                }
            }

            return "No answer";
        }
    }
}