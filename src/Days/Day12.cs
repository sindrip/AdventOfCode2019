using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Days
{
    public class Day12 : IDay
    {
        private List<Moon> _moons;
        public Day12()
        {
            _moons = File.ReadAllLines("input/day12.txt")
                .Select(l => Regex.Matches(l, @"-?[\d]+").Select(m => int.Parse(m.Value)).ToList())
                .Select(coords => new Moon(coords[0], coords[1], coords[2])).ToList();
            
            Console.WriteLine(string.Join(',', _moons));
        }
        
        public string Part1()
        {
            var steps = 1000;
            for (var i = 0; i < steps; i++)
            {
                foreach (var x in _moons)
                {
                    foreach (var y in _moons)
                        x.CalculateAttraction(y);
                }

                foreach (var m in _moons)
                    m.Update();
            }

            return _moons.Aggregate(0, (i, moon) => moon.Energy() + i).ToString();
        }

        public string Part2()
        {
            foreach (var m in _moons)
                m.Reset();

            var xs = _moons.Select(m => (m.X, 0)).ToList();
            var origXs = new List<(int, int)>(xs);
            //var ys = _moons.Select(m => (m.Y, 0));
            //var zs = _moons.Select(m => (m.Z, 0));

            return "";
        }

        private class Moon
        {
            public int X; public int Y; public int Z;
            private int dx; private int dy; private int dz;
            private int initX; private int initY; private int initZ;

            public Moon(int x, int y, int z)
            {
                (initX, initY, initZ) = (x, y, z);
                (X, Y, Z) = (x, y, z);
                (dx, dy, dz) = (0, 0, 0);
            }

            public void CalculateAttraction(Moon other)
            {
                dx += other.X.CompareTo(X);
                dy += other.Y.CompareTo(Y);
                dz += other.Z.CompareTo(Z);
            }

            public int Energy()
            {
                var t1 = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
                var t2 = Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);
                return t1 * t2;
            }

            public void Update()
            {
                X += dx;
                Y += dy;
                Z += dz;
            }

            public bool IsInitial() => (X, Y, Z) == (initX, initY, initZ) && (dx,dy,dz) == (0,0,0);

            public void Reset()
            {
                (X, Y, Z) = (initX, initY, initZ);
                (dx, dy, dz) = (0, 0, 0);
            }

            public override string ToString() => $"pos: ({X},{Y},{Z}), vel:({dx},{dy},{dz})";
        }

    }
}