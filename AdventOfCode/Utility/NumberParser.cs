namespace AdventOfCode.Utility
{
    using System;
    using System.Runtime.CompilerServices;

    public static class NumberParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseInt(ReadOnlySpan<char> input)
        {
            var val = 0;
            foreach (var t in input)
            {
                val = (val * 10) + (t - '0');
            }

            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ParseUlong(ReadOnlySpan<char> input)
        {
            ulong val = 0;
            foreach (var t in input)
            {
                val = (val * 10) + (ulong)(t - '0');
            }

            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ParseUlongBase2(ReadOnlySpan<char> input)
        {
            var val = 0ul;
            for (var i = input.Length - 1; i >= 0; --i)
            {
                var offset = (input.Length - 1) - i;
                var value = (ulong)input[i] - '0';
                val = value | (1UL << offset);
            }

            return val;
        }
    }
}
