using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Days.Extensions;
using Days.IntCode;

namespace Days
{
    public class Day07 : IDay
    {
        private List<BigInteger> _instructions;
        public Day07()
        {
            _instructions = File.ReadAllText("input/day07.txt")
                .Split(',')
                .Select(x => BigInteger.Parse(x)).ToList();
        }
        public string Part1()
        {
            var i = new List<int>() {0, 1, 2, 3, 4};
            var perms = Permutations(i);

            var vms = new List<Vm>
            {
                new Vm(_instructions), new Vm(_instructions), new Vm(_instructions), new Vm(_instructions),
                new Vm(_instructions)
            };

            var max = new BigInteger(int.MinValue);
            foreach (var p in perms)
            {
                vms[0].SetInitialInput(new List<BigInteger>() {p[0], 0});
                vms[0].Reset();
                vms[0].Run();
                vms[1].SetInitialInput(new List<BigInteger>() {p[1], vms[0].Output[0]});
                vms[1].Reset();
                vms[1].Run();
                vms[2].SetInitialInput(new List<BigInteger>() {p[2], vms[1].Output[0]});
                vms[2].Reset();
                vms[2].Run();
                vms[3].SetInitialInput(new List<BigInteger>() {p[3], vms[2].Output[0]});
                vms[3].Reset();
                vms[3].Run();
                vms[4].SetInitialInput(new List<BigInteger>() {p[4], vms[3].Output[0]});
                vms[4].Reset();
                vms[4].Run();
                max = BigInteger.Max(max, vms[4].Output[0]);
            }
            return max.ToString();
        }

        private List<List<T>> Permutations<T>(List<T> list)
        {
            if (list.Count == 1)
                return new List<List<T>>() {list.First().Singleton().ToList()};

            // ToList calls make this unreadable :(
            return list.SelectMany(x =>
                Permutations<T>(list.Where(e => !e.Equals(x)).ToList())
                    .Select(xs => x.Singleton().Concat(xs).ToList()).ToList()
            ).ToList();
        }
        
        public string Part2()
        {
            var i = new List<BigInteger>() {5, 6, 7, 8, 9};
            var perms = Permutations(i);

            var vms = new List<Vm>
            {
                new Vm(_instructions), new Vm(_instructions), new Vm(_instructions), new Vm(_instructions),
                new Vm(_instructions)
            };

            var max = new BigInteger(int.MinValue);
            foreach (var p in perms)
            {
                vms[0].SetInitialInput(p[0].Singleton().ToList());
                vms[1].SetInitialInput(p[1].Singleton().ToList());
                vms[2].SetInitialInput(p[2].Singleton().ToList());
                vms[3].SetInitialInput(p[3].Singleton().ToList());
                vms[4].SetInitialInput(p[4].Singleton().ToList());
                vms[0].AddInput(0);
                do
                {
                    vms[0].Run();
                    vms[1].AddInput(vms[0].LastOutput());
                    vms[1].Run();
                    vms[2].AddInput(vms[1].LastOutput());
                    vms[2].Run();
                    vms[3].AddInput(vms[2].LastOutput());
                    vms[3].Run();
                    vms[4].AddInput(vms[3].LastOutput());
                    vms[4].Run();
                    vms[0].AddInput(vms[4].LastOutput());
                } while (vms.Any(vm => vm.WaitForInput));

                max = BigInteger.Max(max, vms[4].LastOutput());
            }

            return max.ToString();
        }
    }
}