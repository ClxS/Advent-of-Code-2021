namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AoCHelper;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day01 : BaseDay
    {
        private readonly IEnumerable<int> input;

        public Day01()
        {
            this.input = FileUtil.GetIntArray(this.InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            using var enumerator = this.input.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new("Empty file!");
            }

            var increaseCount = 0;
            var lastValue = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current > lastValue)
                {
                    increaseCount++;
                }

                lastValue = enumerator.Current;
            }

            return new(increaseCount.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            using var enumerator = this.input.GetEnumerator();
            
            var valueBuffer = new StackCircularBuffer<int>(stackalloc int[3], 0);
            var sum = 0;
            
            // Initial populate buffer
            for (var i = 0; i < 3; i++, enumerator.MoveNext())
            {
                valueBuffer.PushFront(enumerator.Current);
                sum += enumerator.Current;
            }
            
            valueBuffer.PushFront(enumerator.Current);
            sum += enumerator.Current;
            
            var increaseCount = 0;
            var lastSum = sum;
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
                sum -= valueBuffer.PopBack();
                if (sum > lastSum)
                {
                    increaseCount++;
                }
                
                valueBuffer.PushFront(enumerator.Current);
                lastSum = sum;
            }

            return new(increaseCount.ToString());
        }
    }
}