using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Days
{
    public class Day06: IDay
    {

        private List<(string, string)> _edges;
        private HashSet<string> _vertices;
        private Dictionary<string, List<(string, string)>> _edgeMap;
        private Dictionary<string, HashSet<string>> _adjacency;
        public Day06()
        {
            _edges = File.ReadLines("input/day06.txt").Select(s => s.Split(')')).Select(v => (v[0], v[1])).ToList();
            _vertices = _edges.Aggregate(new HashSet<string>(), (x, y) => x.Union(new HashSet<string>() {y.Item1, y.Item2}).ToHashSet());
            _edgeMap = new Dictionary<string, List<(string,string)>>();
            _adjacency = new Dictionary<string, HashSet<string>>();
            foreach (var v in _vertices)
            {
                _edgeMap.Add(v, _edges.Where(e => e.Item1 == v).ToList());
                var adj = _edges.Where(e => e.Item1 == v).Select(x => x.Item2)
                    .Union(_edges.Where(e => e.Item2 == v).Select(x => x.Item1))
                    .ToHashSet();
                _adjacency.Add(v, adj);
            }
            //Console.WriteLine(string.Join(',' , _edges));
            //PrintDictionary(_edgeMap);
        }

        public string Part1()
        {
            var to = _edges.Select(x => x.Item2).ToList();
            var startNode = _vertices.Where(v => !to.Contains(v)).First();
            return CalculateObits(startNode, 0).ToString();
        }

        private int CalculateObits(string node, int indirect)
        {
            var edges = _edgeMap[node];
            var p = 0;
            foreach (var edge in edges)
            {
                p += CalculateObits(edge.Item2, indirect + 1);
            }
            return p + indirect;
        }

        public string Part2()
        {
            var res = ShortestPath("YOU", "SAN") - 2;
            return res.ToString();
        }

        private int ShortestPath(string start, string target)
        {
            var unvisited = new HashSet<string>(_vertices);
            var dist = new Dictionary<string, int>();
            foreach (var v in _vertices)
            {
                dist.Add(v, int.MaxValue-1);
            }
            dist[start] = 0;

            var current = start;
            while (unvisited.Count > 0)
            {
                foreach (var v in _adjacency[current])
                {
                    var d = dist[current] + 1;
                    dist[v] = Math.Min(dist[v], dist[current] + 1);
                }
                unvisited.Remove(current);
                var next = ("", int.MaxValue);
                foreach (var v in unvisited)
                {
                    if (dist[v] < next.Item2)
                    {
                        next = (v, dist[v]);
                    }
                }

                current = next.Item1;
            }
            return dist[target];
        }
    }
}