namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day06 : FastBaseDay<ulong>
    {
        private readonly string input;
        
        public Day06()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        protected override ulong Solve1()
        {
            var fishCount = this.input.AsSpan().CountCharacters(',') + 1;
            var fishDateOffsets = this.input.Split(",").Select(int.Parse);
            var totalFish = (ulong)fishCount;
            var dayDict = new ConcurrentDictionary<int, ulong>();

            Parallel.ForEach(fishDateOffsets.Distinct(), uniqueDate =>
            {
                dayDict[uniqueDate] = this.SumProducedFish(uniqueDate + 1, 80);
            });
            
            foreach (var fish in fishDateOffsets)
            {
                totalFish += dayDict[fish];
            }

            return totalFish;
        }

        protected override ulong Solve2()
        {
            var fishCount = this.input.AsSpan().CountCharacters(',') + 1;
            var fishDateOffsets = this.input.Split(",").Select(int.Parse);
            var totalFish = (ulong)fishCount;
            var dayDict = new ConcurrentDictionary<int, ulong>();

            Parallel.ForEach(fishDateOffsets.Distinct(), uniqueDate =>
            {
                dayDict[uniqueDate] = this.SumProducedFish(uniqueDate + 1, 256);
            });
            
            foreach (var fish in fishDateOffsets)
            {
                totalFish += dayDict[fish];
            }

            return totalFish;
        }

        private ulong SumProducedFish(int daysUntilNextSpawn, int remainingDays)
        {
            if (remainingDays < daysUntilNextSpawn)
            {
                return 0;
            }

            var spawnedChildren = (remainingDays - daysUntilNextSpawn) / 7;

            var total = 1ul + (ulong)spawnedChildren;
            total += this.SumProducedFish(8 + 1, remainingDays - daysUntilNextSpawn);
            for (var childFish = 1; childFish <= spawnedChildren; ++childFish)
            {
                if (remainingDays - daysUntilNextSpawn - (childFish * 7) < 0)
                {
                    total--;
                    break;
                }
                
                total += this.SumProducedFish(8 + 1, remainingDays - daysUntilNextSpawn - (childFish * 7));
            }

            return total;
        }
    }
}