namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day03 : FastBaseDay<int>
    {
        private readonly string input;

        public Day03()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected override int Solve1()
        {
            var entries = 0;
            
            // Determine input size
            var firstLength = this.input.IndexOf('\n');
            
            Span<int> oneCounts = stackalloc int[firstLength];
            var mask = MaskUtil.SetOnes(firstLength);
            
            foreach (var number in this.input.SplitLines())
            {
                entries++;
                for (var idx = 0; idx < number.Length; idx++)
                {
                    oneCounts[idx] += number[idx] - '0';
                }
            }

            var halfEntries = entries / 2;
            var gammaRate = 0;
            for (var idx = 0; idx < oneCounts.Length; idx++)
            {
                gammaRate |= (oneCounts[oneCounts.Length - 1 - idx] > halfEntries ? 1 : 0) << idx;
            }

            var epsilonRate = (gammaRate ^ mask) & mask;
            return gammaRate * epsilonRate;
        }

        protected override int Solve2()
        {
            var entries = 0;
            
            // Determine input size
            var firstLength = this.input.IndexOf('\n');
            
            Span<int> oxygenNumbers = stackalloc int[this.input.CountCharacters('\n') + 1];
            Span<int> scrubberNumbers = stackalloc int[oxygenNumbers.Length];
            
            foreach (var number in this.input.SplitLines())
            {
                oxygenNumbers[entries] = NumberParser.ParseBinary(number);
                scrubberNumbers[entries] = oxygenNumbers[entries];
                entries++;
            }

            var targetMask = FindComplexValue(oxygenNumbers, firstLength, false);
            var co2 = FindComplexValue(scrubberNumbers, firstLength, true);

            return targetMask * co2;
        }

        private static int FindComplexValue(Span<int> numbers, int numberLength, bool invert)
        {
            var remainingNumbers = numbers.Length;
            for (var step = 0; step < numberLength; step++)
            {
                var popCount = 0;
                foreach (var num in numbers)
                {
                    if (num < 0)
                    {
                        continue;
                    }
                    
                    popCount += num >> (numberLength - step - 1) & 0b1;
                }

                var isPositive = (popCount >= remainingNumbers / 2.0);
                if (invert)
                {
                    isPositive = !isPositive;
                }
                
                if (isPositive)
                {
                    for (var i = 0; i < numbers.Length; ++i)
                    {
                        if (numbers[i] < 0)
                        {
                            continue;
                        }

                        var bit = numbers[i] >> (numberLength - step - 1) & 0b1;
                        if (bit == 0)
                        {
                            numbers[i] = -1;
                            remainingNumbers--;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < numbers.Length; ++i)
                    {
                        if (numbers[i] < 0)
                        {
                            continue;
                        }

                        var bit = numbers[i] >> (numberLength - step - 1) & 0b1;
                        if (bit != 0)
                        {
                            numbers[i] = -1;
                            remainingNumbers--;
                        }
                    }
                }

                if (remainingNumbers == 1)
                {
                    foreach (var number in numbers)
                    {
                        if (number < 0)
                        {
                            continue;
                        }

                        return number;
                    }
                }
            }

            return -1;
        }
    }
}