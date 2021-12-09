namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Intrinsics;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Toolkit.HighPerformance;
    using Perfolizer.Horology;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public unsafe sealed class Day09 : FastBaseDay<int>
    {
        private readonly string input;
        
        public Day09()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected override int Solve1()
        {
            var fileBytes = Encoding.ASCII.GetBytes(this.input);
            fixed (void* ptr = fileBytes)
            {
                var width = this.input.IndexOf('\n');
                var map = new Span2D<byte>(ptr, this.input.Length / (width + 1) + 1, width, 1);
                for (var x = 0; x < map.Width; x++)
                {
                    for (var y = 0; y < map.Height; y++)
                    {
                        map[y, x] -= 48; // ascii '0'
                    }
                }

                var count = 0;
                for (var x = 0; x < map.Width; x++)
                {
                    for (var y = 0; y < map.Height; y++)
                    {
                        var thisValue = map[y, x];
                        if (
                            (x == 0 || map[y, x - 1] > thisValue) &&
                            (x == map.Width - 1 || map[y, x + 1] > thisValue) &&
                            (y == 0 || map[y - 1, x] > thisValue) &&
                            (y == map.Height - 1 || map[y + 1, x] > thisValue))
                        {
                            count += thisValue + 1;
                        }
                    }
                }

                return (int)count;
            }
        }

        protected override int Solve2()
        {
            Span<int> threeLargest = stackalloc int[3];
            var fileBytes = Encoding.ASCII.GetBytes(this.input);
            var tmpFloodMap = new byte[fileBytes.Length];
            fixed (byte* ptr = fileBytes)
            fixed (byte* floodMapBytes = tmpFloodMap)
            {
                var width = this.input.IndexOf('\n');
                var map = new Span2D<byte>(ptr, this.input.Length / (width + 1) + 1, width, 1);
                var floodMap = new Span2D<byte>(floodMapBytes, this.input.Length / (width + 1) + 1, width, 1);

                for (var x = 0; x < map.Width; x++)
                {
                    for (var y = 0; y < map.Height; y++)
                    {
                        map[y, x] -= 48; // ascii '0'
                    }
                }

                for (var x = 0; x < map.Width; x++)
                {
                    for (var y = 0; y < map.Height; y++)
                    {
                        var thisValue = map[y, x];
                        if (
                            (x == 0 || map[y, x - 1] > thisValue) &&
                            (x == map.Width - 1 || map[y, x + 1] > thisValue) &&
                            (y == 0 || map[y - 1, x] > thisValue) &&
                            (y == map.Height - 1 || map[y + 1, x] > thisValue))
                        {
                            floodMap.Fill(0);
                            var size = this.FillBasin(x, y, map, floodMap);
                            var minValue = FindMin(threeLargest, out var minIndex);
                            if (size > minValue)
                            {
                                threeLargest[minIndex] = size;
                            }
                        }
                    }
                }

                return threeLargest[0] * threeLargest[1] * threeLargest[2];
            }
        }

        private static int FindMin(Span<int> numbers, out int smallestIdx)
        {
            smallestIdx = 0;
            for (var index = 1; index < numbers.Length; index++)
            {
                if (numbers[smallestIdx] > numbers[index])
                {
                    smallestIdx = index;
                }
            }

            return numbers[smallestIdx];
        }

        private int FillBasin(int x, int y, Span2D<byte> map, Span2D<byte> floodBasin)
        {
            var positionsToCheck = new StackCircularBuffer<(int X, int Y)>(stackalloc (int, int)[1024], 0);
            positionsToCheck.PushBack((x, y));

            var size = 0;
            while (!positionsToCheck.IsEmpty)
            {
                (x, y) = positionsToCheck.PopFront();
                if (floodBasin[y, x] == 1)
                {
                    continue;
                }

                size++;
                floodBasin[y, x] = 1;
                if (x > 0 && map[y, x - 1] != 9 && floodBasin[y, x - 1] == 0)
                {
                    positionsToCheck.PushBack((x - 1, y));
                }
                
                if (x < map.Width - 1 && map[y, x + 1] != 9 && floodBasin[y, x + 1] == 0)
                {
                    positionsToCheck.PushBack((x + 1, y));
                }
                
                if (y > 0 && map[y - 1, x] != 9 && floodBasin[y - 1, x] == 0)
                {
                    positionsToCheck.PushBack((x, y - 1));
                }
                
                if (y < map.Height - 1 && map[y + 1, x] != 9 && floodBasin[y + 1, x] == 0)
                {
                    positionsToCheck.PushBack((x, y + 1));
                }
            }

            return size;
        }
    }
}