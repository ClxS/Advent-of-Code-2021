namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Runtime.Intrinsics;
    using System.Threading.Tasks;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day08 : FastBaseDay<ulong>
    {
        private readonly string input;

        public delegate bool MaskDelegate(uint digit, Span<uint> knownMasks);

        private static readonly MaskDelegate[] NumberLookups = {
            /* 0 */ (d, m) => BitOperations.PopCount(d) == 6 && HasNMatching(d, m[4], 3) && HasNMatching(d, m[1], 2),
            /* 1 */ (d, m) => d == m[1],
            /* 2 */ (d, m) => BitOperations.PopCount(d) == 5 && HasNDiffs(d, m[4], 3),
            /* 3 */ (d, m) => BitOperations.PopCount(d) == 5 && HasNMatching(d, m[1], 2),
            /* 4 */ (d, m) => d == m[4],
            /* 5 */ (d, m) => BitOperations.PopCount(d) == 5 && HasNDiffs(d, m[4], 2),
            /* 6 */ (d, m) => BitOperations.PopCount(d) == 6 && HasNDiffs(d, m[4], 3),
            /* 7 */ (d, m) => d == m[7],
            /* 8 */ (d, m) => d == m[8],
            /* 9 */ (d, m) => BitOperations.PopCount(d) == 6 && HasNMatching(d, m[4], 4),
        };
        
        public Day08()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected override ulong Solve1()
        {
            var sum = 0ul;
            foreach (var line in this.input.SplitLines())
            {
                var reader = new SpanStringReader(line);
                reader.ReadUntil('|', false);
                reader.Skip(2);

                while (!reader.IsEndOfFile())
                {
                    var word = reader.ReadWord();
                    sum += word.Length is 2 or 3 or 4 or 7 ? 1ul : 0ul;
                }
            }

            return sum;
        }

        protected override ulong Solve2()
        {
            var sum = 0ul;
            Span<int> occurence = stackalloc int[10];
            Span<uint> knownMasks = stackalloc uint[10];

            foreach (var line in this.input.SplitLines())
            {
                var reader = new SpanStringReader(line);
                for (var i = 0; i < 10; i++)
                {
                    var word = reader.ReadWord();
                    var mask = this.CalcMask(word);
                    switch (word.Length)
                    {
                        case 2:
                            knownMasks[1] = mask;
                            break;
                        case 3:
                            knownMasks[7] = mask;
                            break;
                        case 4:
                            knownMasks[4] = mask;
                            break;
                        case 7:
                            knownMasks[8] = mask;
                            break;
                    }
                }

                reader.ReadUntil('|', false);
                reader.Skip(2);

                var localSum = 0ul;
                while (!reader.IsEndOfFile())
                {
                    var word = reader.ReadWord();
                    var mask = this.CalcMask(word);

                    for (var idx = 0; idx < NumberLookups.Length; idx++)
                    {
                        var lookup = NumberLookups[idx];
                        if (lookup(mask, knownMasks))
                        {
                            localSum = (localSum * 10) + (ulong)idx;
                            occurence[idx]++;
                            break;
                        }
                    }
                }

                sum += localSum;
            }

            return sum;
        }

        private static bool HasNMatching(uint currentDigit, uint mask, int expectation)
        {
            return BitOperations.PopCount(currentDigit & mask) == expectation;
        }

        private static bool HasNDiffs(uint currentDigit, uint mask, int expectation)
        {
            return BitOperations.PopCount(currentDigit & (~mask)) == expectation;
        }

        private uint CalcMask(ReadOnlySpan<char> word)
        {
            var mask = 0u;
            foreach (var @char in word)
            {
                mask |= (1u << (@char - 'a'));
            }

            return mask;
        }
    }
}