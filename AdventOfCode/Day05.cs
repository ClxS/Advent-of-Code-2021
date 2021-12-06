namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day05 : FastBaseDay<int>
    {
        private readonly string input;

        public Day05()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected override int Solve1()
        {
            var overlapCells = new HashSet<(int X, int Y)>();
            var cell = new short[1000, 1000];
            foreach (var line in this.input.SplitLines())
            {
                ReadLine(line, out var x1, out var y1, out var x2, out var y2);
                if (x1 != x2 && y1 != y2)
                {
                    continue;
                }

                for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                {
                    for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                    {
                        cell[x, y]++;
                        if (cell[x, y] == 2)
                        {
                            overlapCells.Add((x, y));
                        }
                    }                    
                }

            }
            
            return overlapCells.Count;
        }

        protected override int Solve2()
        {
            
            var overlapCells = new HashSet<(int X, int Y)>();
            var cell = new short[1000, 1000];
            foreach (var line in this.input.SplitLines())
            {
                ReadLine(line, out var x1, out var y1, out var x2, out var y2);
                var xVelocity = Math.Sign(x2 - x1);
                var yVelocity = Math.Sign(y2 - y1);

                while (x1 != (x2 + xVelocity) || y1 != (y2 + yVelocity))
                {
                    cell[x1, y1]++;
                    if (cell[x1, y1] == 2)
                    {
                        overlapCells.Add((x1, y1));
                    }

                    x1 += xVelocity;
                    y1 += yVelocity;
                }
            }
            
            return overlapCells.Count;
        }

        private static void ReadLine(ReadOnlySpan<char> line, out int x1, out int y1, out int x2, out int y2)
        {
            var reader = new SpanStringReader(line);
            x1 = reader.ReadInt(false);
            reader.Skip(1); // ","
            y1 = reader.ReadInt(false);
            reader.Skip(4); // " -> " 
            x2 = reader.ReadInt(false);
            reader.Skip(1); // ","
            y2 = reader.ReadInt(false);
        }
    }
}