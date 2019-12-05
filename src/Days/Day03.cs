using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace Days
{
    public class Day03: IDay
    {
        private List<(int, int)> _wire1Movements;
        private List<(int, int)> _wire2Movements;
        private List<(int, int)> _wire1;
        private List<(int, int)> _wire2;
        private HashSet<(int, int)> _common;
        public Day03()
        {
            var wires = File.ReadLines("input/day03.txt").ToList();
            _wire1Movements = ParseWireMovement(wires[0]);
            _wire2Movements = ParseWireMovement(wires[1]);
            
            _wire1 = new List<(int, int)>();
            _wire2 = new List<(int, int)>();
            var currStep = (0, 0);
            foreach (var mvmt in _wire1Movements)
            {
                var nextStep = (currStep.Item1 + mvmt.Item1, currStep.Item2 + mvmt.Item2);
                _wire1.Add(nextStep);
                currStep = nextStep;
            }

            currStep = (0, 0);
            foreach (var mvmt in _wire2Movements)
            {
                var nextStep = (currStep.Item1 + mvmt.Item1, currStep.Item2 + mvmt.Item2);
                _wire2.Add(nextStep);
                currStep = nextStep;
            }

            _common = _wire1.Intersect(_wire2).ToHashSet();
        }

        private List<(int, int)> ParseWireMovement(string inputWire)
        {
            var wireMovements = new List<(int, int)>();
            var steps = inputWire.Split((',')).Select(s => (s.Substring(0, 1), s.Substring((1))));
            foreach (var (dir, length) in steps)
            {
                var l = int.Parse(length);
                wireMovements.AddRange(dir switch
                {
                    "L" => Enumerable.Repeat((-1,0), l).ToList(),
                    "R" => Enumerable.Repeat((1,0), l).ToList(),
                    "U" => Enumerable.Repeat((0,1), l).ToList(),
                    "D" => Enumerable.Repeat((0,-1), l).ToList(),
                });
            };
            return wireMovements;
        }

        public string Part1() => _common.Select(x => Math.Abs(x.Item1) + Math.Abs((x.Item2))).Min().ToString();

        public string Part2()
        {
            var wire1 = new List<(int, int, int)>();
            var wire2 = new List<(int, int, int)>();
            var currStep = (0, 0, 0);
            foreach (var mvmt in _wire1Movements)
            {
                var nextStep = (currStep.Item1 + mvmt.Item1, currStep.Item2 + mvmt.Item2, currStep.Item3 + 1);
                if (_common.Contains((nextStep.Item1, nextStep.Item2)))
                    wire1.Add(nextStep);
                currStep = nextStep;
            }

            currStep = (0, 0, 0);
            foreach (var mvmt in _wire2Movements)
            {
                var nextStep = (currStep.Item1 + mvmt.Item1, currStep.Item2 + mvmt.Item2, currStep.Item3 + 1);
                if (_common.Contains((nextStep.Item1, nextStep.Item2)))
                    wire2.Add(nextStep);
                currStep = nextStep;
            }

            var cross = from x in wire1
                from y in wire2
                where x.Item1 == y.Item1
                      && x.Item2 == y.Item2
                select x.Item3 + y.Item3;

            return cross.Min().ToString();
        }
    }
}