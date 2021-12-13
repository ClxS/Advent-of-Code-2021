namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Toolkit.HighPerformance;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed unsafe class Day12 : FastBaseDay<int>
    {
        protected override int Solve1()
        {
            var map = new Dictionary<string, Node>();
            var startNode = new Node() { Name = "start", IsSmall = true };
            var endNode = new Node() { Name = "end", IsSmall = true };
            map["start"] = startNode;
            map["end"] = endNode;
            
            foreach (var line in this.Input.EnumerateLines())
            {
                var idx = line.IndexOf('-');
                var a = line[0..idx];
                var b = line[(idx + 1)..];

                if (!map.TryGetValue(a.ToString(), out var aNode))
                {
                    aNode = new() { Name = a.ToString(), IsSmall = char.IsLower(a[0]) };
                    map[a.ToString()] = aNode;
                }
                
                if (!map.TryGetValue(b.ToString(), out var bNode))
                {
                    bNode = new() { Name = b.ToString(), IsSmall = char.IsLower(b[0]) };
                    map[b.ToString()] = bNode;
                }

                aNode.Adjacents.Add(bNode);
                bNode.Adjacents.Add(aNode);
            }

            var routeTree = new List<Route>()
            {
                new(startNode)
            };
            var finishedRoutes = new List<Route>();

            while (routeTree.Count > 0)
            {
                for (var idx = routeTree.Count - 1; idx >= 0; idx--)
                {
                    var subtree = routeTree[idx];
                    routeTree.RemoveAt(idx);
                    
                    var node = subtree.Nodes.Last();
                    foreach (var neighbour in node.Adjacents.Where(a => !a.IsSmall || !subtree.Nodes.Contains(a)))
                    {
                        if (neighbour.Name == "end")
                        {
                            finishedRoutes.Add(new(subtree.Nodes, neighbour, false));
                        }
                        else
                        {
                            routeTree.Add(new(subtree.Nodes, neighbour, false));
                        }
                    }
                }
            }
            
            return finishedRoutes.Count;
        }

        protected override int Solve2()
        {
            var map = new Dictionary<string, Node>();
            var startNode = new Node() { Name = "start", IsSmall = true };
            var endNode = new Node() { Name = "end", IsSmall = true };
            map["start"] = startNode;
            map["end"] = endNode;
            
            foreach (var line in this.Input.EnumerateLines())
            {
                var idx = line.IndexOf('-');
                var a = line[0..idx];
                var b = line[(idx + 1)..];

                if (!map.TryGetValue(a.ToString(), out var aNode))
                {
                    aNode = new() { Name = a.ToString(), IsSmall = char.IsLower(a[0]) };
                    map[a.ToString()] = aNode;
                }
                
                if (!map.TryGetValue(b.ToString(), out var bNode))
                {
                    bNode = new() { Name = b.ToString(), IsSmall = char.IsLower(b[0]) };
                    map[b.ToString()] = bNode;
                }

                aNode.Adjacents.Add(bNode);
                bNode.Adjacents.Add(aNode);
            }

            var routeTree = new List<Route>()
            {
                new(startNode)
            };
            var finishedRoutes = new List<Route>();

            while (routeTree.Count > 0)
            {
                for (var idx = routeTree.Count - 1; idx >= 0; idx--)
                {
                    var subtree = routeTree[idx];
                    routeTree.RemoveAt(idx);
                    
                    var node = subtree.Nodes.Last();
                    foreach (var neighbour in node.Adjacents)
                    {
                        if (neighbour.Name != "end" && (neighbour.IsSmall && subtree.HasSmallNodeRevisit&& subtree.Nodes.Contains(neighbour)) || neighbour.Name == "start")
                        {
                            continue;
                        }
                        
                        if (neighbour.Name == "end")
                        {
                            finishedRoutes.Add(new(subtree.Nodes, neighbour, subtree.HasSmallNodeRevisit));
                        }
                        else
                        {
                            routeTree.Add(new(subtree.Nodes, neighbour, subtree.HasSmallNodeRevisit || (neighbour.IsSmall && subtree.Nodes.Contains(neighbour))));
                        }
                    }
                }
            }

            return finishedRoutes.Count;
        }
        
        private class Route
        {
            public List<Node> Nodes { get; set; } = new();

            public bool HasSmallNodeRevisit { get; set; }

            public Route(IEnumerable<Node> prior, Node next, bool hasSmallNodeRevisit)
            {
                this.HasSmallNodeRevisit = hasSmallNodeRevisit;
                this.Nodes.AddRange(prior);
                this.Nodes.Add(next);
            }
            
            public Route(Node next)
            {
                this.Nodes.Add(next);
            }

            public override string ToString()
            {
                return string.Join(',', this.Nodes.Select(n => n.Name).ToArray());
            }
        }
    
        private class Node
        {
            public HashSet<Node> Adjacents { get; } = new HashSet<Node>();
    
            public string Name { get; set; }
    
            public bool IsSmall { get; set; }
        }
    }
}