using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Days
{
    public class Day10: IDay
    {
        private List<Point> _asteroids;
        public Day10()
        {
            _asteroids = File.ReadAllLines("input/day10.txt")
                .SelectMany((l, i) => l.Select((c,j) => (c, i , j)))
                .Where(x => x.c == '#')
                .Select(x => new Point(x.j, x.i))
                .Select(x => x.Rotate())
                .ToList();
        }
        public string Part1()
        {
            var slopes = _asteroids.Select(a => _asteroids.Where(x => !a.Equals(x))
                .Select(b => new Ratio(b.Y - a.Y, b.X - a.X)).ToHashSet());

            return slopes.Select(x => x.Count).Max().ToString();
        }

        public string Part2()
        {
            var optimalPoint = _asteroids.Select(a => _asteroids.Where(x => !a.Equals(x))
                    .Select(b => (a, new Ratio(b.Y - a.Y, b.X - a.X))).ToHashSet()
                ).Select(x => (x.First().a, x.Count))
                .Aggregate((new Point(0, 0), int.MinValue), (x, y) => x.MinValue < y.Count ? y : x).Item1;

            var slopes = _asteroids.Where(a => !a.Equals(optimalPoint))
                .Select(a => (a, Math.Abs(optimalPoint.X - a.X) + Math.Abs(optimalPoint.Y - a.Y),
                    new Ratio(a.Y - optimalPoint.Y, a.X - optimalPoint.X).Atan2()))
                .OrderBy(x => x.Item3)
                .ThenBy(x => x.Item2)
                .GroupBy(x => x.Item3)
                .ToDictionary(x => x.Key, x => x.ToList());

            var i = 0;
            var keys = slopes.Keys;
            while (slopes.Any(x => x.Value.Count > 0))
            {
                foreach (var k in keys)
                {
                    if (slopes[k].Count == 0)
                        continue;

                    i++;
                    var p = slopes[k].First();
                    if (i == 200)
                    {
                        var r = p.a.Rotate().Rotate().Rotate();
                        return (r.X * 100 + r.Y).ToString();
                    }
                    slopes[k].Remove(p);
                }
            }
            return ":(";
        }

        private struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            // Rotate the point so that we can order the ratios by ArcTan
            public Point Rotate() => new Point(Y, -X);
            public override string ToString() => $"({X}, {Y})";
        }

        private struct Ratio : IComparable<Ratio>
        {
            public int Numerator;
            public int Denominator;

            public Ratio(int n, int d)
            {
                var gcd = GCD(n, d);
                Numerator = n / gcd;
                Denominator = d / gcd;
            }

            public int CompareTo(Ratio other) => Atan2().CompareTo(other.Atan2());

            public double Atan2() 
            {
                var a = Math.Atan2((double) Numerator, (double) Denominator);
                if (a >= 3.14)
                    return -(2 * Math.PI);
                return a;
            }

            public override string ToString() => $"({Numerator} / {Denominator})";
        }

        private static int GCD(int n, int d) => d == 0 ? Math.Abs(n) : GCD(d, n % d);
        
    }
}