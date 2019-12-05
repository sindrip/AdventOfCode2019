using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Days.IntCode;
using Microsoft.VisualBasic;

namespace Days
{
    public class Day05 : IDay
    {
        private List<int> _nums;
        public Day05()
        {
            _nums = File.ReadAllText("input/day05.txt")
                .Split(',')
                .Select(x => int.Parse(x)).ToList();
        }
        public string Part1()
        {
            return "";
            var input = new List<int>() {1};
            var vm = new Vm(_nums, input);
            vm.Run();
            return string.Join(',', vm.Output);
        }

        public string Part2()
        {
            //var nums = new List<int>() {3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
            //var nums = new List<int>() {3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9};
            //var nums = new List<int>() {3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1};
            //var nums = new List<int>() {3,3,1108,-1,8,3,4,3,99};
            var input = new List<int>() {5};
            var vm = new Vm(_nums, input);
            vm.Run();
            return string.Join(',', vm.Output);
        }
    }
}