namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AoCHelper;
    using Utility;

    public sealed class Day01 : BaseDay
    {
        private readonly IReadOnlyList<int> input;

        public Day01()
        {
            this.input = FileUtil.GetIntArray(this.InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var increaseCount = 0;
            for (var i = 1; i < this.input.Count; ++i)
            {
                if (this.input[i] > this.input[i - 1])
                {
                    increaseCount++;
                }
            }

            return new(increaseCount.ToString());
        }

        public override ValueTask<string> Solve_2()
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

            return new(increaseCount.ToString());
        }
    }
}