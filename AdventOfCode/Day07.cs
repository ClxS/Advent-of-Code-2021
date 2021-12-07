namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Intrinsics;
    using System.Threading.Tasks;
    using Perfolizer.Horology;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day07 : FastBaseDay<ulong>
    {
        private string input;
        
        public Day07()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        protected override ulong Solve1()
        {
            Span<int> crabs = stackalloc int[this.input.AsSpan().CountCharacters(',') + 1];
            this.ReadCrabs(crabs);
            crabs.Sort();
            var targetPoint = crabs[crabs.Length / 2];

            var fuelSum = 0;
            foreach (var crab in crabs)
            {
                fuelSum += Math.Abs(crab - targetPoint);
            }
            
            return (ulong)fuelSum;
        }

        protected override ulong Solve2()
        {
            Span<int> crabs = stackalloc int[this.input.AsSpan().CountCharacters(',') + 1];
            this.ReadCrabs(crabs);
            
            var sum = 0;
            foreach (var crab in crabs)
            {
                sum += crab;
            }

            var minTarget = (int)Math.Floor(sum / (float)crabs.Length);
            var maxTarget = (int)Math.Ceiling(sum / (float)crabs.Length);

            var minFuelSum = 0;
            foreach (var crab in crabs)
            {
                minFuelSum += NumberUtility.TriangleNumber(Math.Abs(crab - minTarget));
            }
            
            var maxFuelSum = 0;
            foreach (var crab in crabs)
            {
                maxFuelSum += NumberUtility.TriangleNumber(Math.Abs(crab - maxTarget));
            }
            
            return (ulong)Math.Min(minFuelSum, maxFuelSum);
        }

        private void ReadCrabs(Span<int> crabs)
        {
            var crabIdx = 0;
            var first = true;
            var reader = new SpanStringReader(this.input);
            
            while (!reader.IsEndOfFile())
            {
                if (!first)
                {
                    reader.Skip(1);
                }
                else
                {
                    first = false;
                }

                crabs[crabIdx++] = reader.ReadInt();
            }
        }
    }
}