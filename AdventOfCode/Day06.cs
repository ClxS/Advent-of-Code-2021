namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Intrinsics;
    using System.Threading.Tasks;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public unsafe sealed class Day06 : FastBaseDay<ulong>
    {
        private readonly string input;
        
        public Day06()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        protected override ulong Solve1()
        {
            return this.SolveBase(80);
        }

        protected override ulong Solve2()
        {
            return this.SolveBase(256);
        }
        
        private ulong SolveBase(int iterations)
        {
            var totalFish = 0ul;
            fixed (char* p = input)
            {
                var str = p;
                var inputLength = this.input.Length;
                var initialCounts = stackalloc ulong[9];
                var end = str + inputLength;
                while(str < end)
                {
                    initialCounts[str[0] - '0']++;
                    totalFish++;
                    str += 2;
                }

                var pivot = 0;
                for (var i = 1; i <= iterations; ++i)
                {
                    var count = initialCounts[pivot];
                    totalFish += count;
                    initialCounts[(pivot + 7) % 9] += count;
                    pivot = (pivot + 1) % 9;
                }
            }

            return totalFish;
        }
    }
}