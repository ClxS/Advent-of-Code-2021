namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Toolkit.HighPerformance;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed unsafe class Day11 : FastBaseDay<int>
    {
        protected override int Solve1()
        {
            var flashCount = 0;
            fixed (char* ptr = this.InputString)
            {
                var map = new Span2D<char>(ptr, 10, 10, 1);
                for (var step = 0; step < 100; step++)
                {
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            map[y, x]++;
                            this.CheckForFlashes(map, x, y, ref flashCount);
                        }
                    }
                    
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            if (map[y, x] < '0')
                            {
                                map[y, x] = '0';
                            }
                        }
                    }
                }
            }

            return flashCount;
        }

        protected override int Solve2()
        {
            var flashCount = 0;
            fixed (char* ptr = this.InputString)
            {
                var map = new Span2D<char>(ptr, 10, 10, 1);
                var step = 1;
                while(true)
                {
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            map[y, x]++;
                            this.CheckForFlashes(map, x, y, ref flashCount);
                        }
                    }

                    flashCount = 0;
                    for (var y = 0; y < 10; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            if (map[y, x] < '0')
                            {
                                map[y, x] = '0';
                                flashCount++;
                            }
                        }
                    }

                    if (flashCount == 100)
                    {
                        return step;
                    }
                    
                    step++;
                }
            }

            return flashCount;
        }

        private void CheckForFlashes(Span2D<char> map, int x, int y, ref int flashCount)
        {
            if (map[y, x] <= '9')
            {
                return;
            }

            map[y, x] = '\0';
            flashCount++;
            for (var checkY = y - 1; checkY <= y + 1; checkY++)
            {
                if (checkY < 0 || checkY >= map.Height)
                {
                    continue;
                }
                
                for (var checkX = x - 1; checkX <= x + 1; checkX++)
                {
                    if (checkX < 0 || checkX >= map.Width)
                    {
                        continue;
                    }
                    
                    map[checkY, checkX]++;
                    this.CheckForFlashes(map, checkX, checkY, ref flashCount);
                }
            }
        }

        private void PrintMap(Span2D<char> map)
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Console.Write(map[y, x]);
                }
                
                Console.WriteLine();
            }
            
            Console.WriteLine();
        }
    }
}