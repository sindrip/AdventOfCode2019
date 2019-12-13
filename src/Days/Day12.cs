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

            var i = 0;
            var x = 0;
            var y = 0;
            var z = 0;
            while (x == 0 || y == 0 || z == 0)
            {
                i++;
                foreach (var m1 in _moons)
                {
                    foreach (var m2 in _moons)
                        m1.CalculateAttraction(m2);
                }

                foreach (var m in _moons)
                    m.Update();

                if (_moons.All(m => m.IsInitialX()))
                    x = i;
                if (_moons.All(m => m.IsInitialY()))
                    y = i;
                if (_moons.All(m => m.IsInitialZ()))
                    z = i;
            }

            return LCM(x, LCM(y, z)).ToString();

            return $"{x}, {y}, {z}";
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

            public bool IsInitialX() => X == initX && dx == 0;
            public bool IsInitialY() => Y == initY && dy == 0;
            public bool IsInitialZ() => Z == initZ && dz == 0;

            public void Reset()
            {
                (X, Y, Z) = (initX, initY, initZ);
                (dx, dy, dz) = (0, 0, 0);
            }

            public override string ToString() => $"pos: ({X},{Y},{Z}), vel:({dx},{dy},{dz})";
        }
        
        private static BigInteger GCD(BigInteger n, BigInteger d) => d == 0 ? BigInteger.Abs(n) : GCD(d, n % d);
        private static BigInteger LCM(BigInteger n, BigInteger d) => (n * d) / GCD(n, d);

    }
}