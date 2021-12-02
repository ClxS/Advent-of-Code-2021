namespace AdventOfCode
{
    using System.Collections.Generic;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day01 : FastBaseDay<int>
    {
        private readonly IReadOnlyList<int> input;

        public Day01()
        {
            this.input = FileUtil.GetIntArray(this.InputFilePath);
        }

        protected override int Solve1()
        {
            var increaseCount = 0;
            for (var i = 1; i < this.input.Count; ++i)
            {
                if (this.input[i] > this.input[i - 1])
                {
                    increaseCount++;
                }
            }

            return increaseCount;
        }

        protected override int Solve2()
        {
            var increaseCount = 0;
            var dataSize = this.input.Count;
            for (var i = 3; i < dataSize; ++i)
            {
                if (this.input[i] > this.input[i - 3])
                {
                    increaseCount++;
                }
            }

            return increaseCount;
        }
    }
}