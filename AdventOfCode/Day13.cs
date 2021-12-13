namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Toolkit.HighPerformance;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed unsafe class Day13 : FastBaseDay<string>
    {
        protected override string Solve1()
        {
            var isFoldPart = false;
            var cells = new Dictionary<(int X, int Y), bool>();
            var folds = new List<(bool IsX, int Position)>();
            foreach (var line in this.Input.EnumerateLines())
            {
                if (line.IsEmpty)
                {
                    isFoldPart = true;
                    continue;
                }

                var reader = new SpanStringReader(line);
                if (isFoldPart)
                {
                    reader.ReadWord();
                    reader.ReadWord();
                    var axis = reader.ReadChar();
                    reader.Skip(1);
                    var position = reader.ReadInt();
                    folds.Add((axis == 'x', position));
                }
                else
                {
                    var x = reader.ReadInt();
                    reader.Skip(1);
                    var y = reader.ReadInt();

                    cells[(x, y)] = true;
                }
            }

            foreach (var (isX, position) in folds)
            {
                if (isX)
                {
                    foreach (var pos in cells.Keys.ToArray())
                    {
                        if (pos.X > position)
                        {
                            cells.Remove(pos);
                            cells[(position - (pos.X - position), pos.Y)] = true;
                        }
                    }
                }
                else
                {
                    foreach (var pos in cells.Keys.ToArray())
                    {
                        if (pos.Y > position)
                        {
                            cells.Remove(pos);
                            cells[(pos.X, position - (pos.Y - position))] = true;
                        }
                    }
                }

                break;
            }

            var maxHeight = cells.Keys.Max(c => c.Y);
            var maxWidth = cells.Keys.Max(c => c.X);
            var output = string.Join('\n', Enumerable.Range(0, maxHeight + 1).Select(v => new string('.', maxWidth + 1)));
            fixed (char* ptr = output)
            {
                var map = new Span2D<char>(ptr, maxHeight + 1, maxWidth + 1, 1);
                foreach (var (key, _) in cells)
                {
                    map[key.Y, key.X] = '#';
                }
            }

            return output.Count(c => c == '#').ToString();
        }

        protected override string Solve2()
        {
            var isFoldPart = false;
            var cells = new Dictionary<(int X, int Y), bool>();
            var folds = new List<(bool IsX, int Position)>();
            foreach (var line in this.Input.EnumerateLines())
            {
                if (line.IsEmpty)
                {
                    isFoldPart = true;
                    continue;
                }

                var reader = new SpanStringReader(line);
                if (isFoldPart)
                {
                    reader.ReadWord();
                    reader.ReadWord();
                    var axis = reader.ReadChar();
                    reader.Skip(1);
                    var position = reader.ReadInt();
                    folds.Add((axis == 'x', position));
                }
                else
                {
                    var x = reader.ReadInt();
                    reader.Skip(1);
                    var y = reader.ReadInt();

                    cells[(x, y)] = true;
                }
            }

            foreach (var (isX, position) in folds)
            {
                if (isX)
                {
                    foreach (var pos in cells.Keys.ToArray())
                    {
                        if (pos.X >= position)
                        {
                            cells.Remove(pos);
                            cells[(position - (pos.X - position), pos.Y)] = true;
                        }
                    }
                }
                else
                {
                    foreach (var pos in cells.Keys.ToArray())
                    {
                        if (pos.Y >= position)
                        {
                            cells.Remove(pos);
                            cells[(pos.X, position - (pos.Y - position))] = true;
                        }
                    }
                }
            }

            var minHeight = cells.Keys.Min(c => c.Y);
            var minWidth = cells.Keys.Min(c => c.X);
            var maxHeight = cells.Keys.Max(c => c.Y);
            var maxWidth = cells.Keys.Max(c => c.X);
            
            var output = string.Join('\n', Enumerable.Range(0, maxHeight + 1 - minHeight).Select(v => new string('.', maxWidth + 1 - minWidth)));
            fixed (char* ptr = output)
            { 
                var map = new Span2D<char>(ptr, maxHeight + 1 - minHeight, maxWidth + 1 - minWidth, 1);
                foreach (var (key, _) in cells)
                {
                    map[key.Y - minHeight, key.X - minWidth] = '#';
                }
                
                PrintMap(map);
            }
            
            return "";
        }

        private static void PrintMap(Span2D<char> buffer)
        {
            for (var y = 0; y < buffer.Height; y++)
            {
                for (var x = 0; x < buffer.Width; x++)
                {
                    Console.Write(buffer[y, x] == '#' ? '#' : '.');
                }
                
                Console.WriteLine();
            }
        }
    }
}