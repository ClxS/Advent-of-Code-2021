namespace AdventOfCode.Utility
{
    using System;
    using System.Runtime.CompilerServices;

    public ref struct StackLongBucketSet
    {
        private readonly Span<long> buffer;

        public StackLongBucketSet(Span<long> buffer)
        {
            this.buffer = buffer;
            this.HighestValue = 0;
        }

        public long this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.buffer[index];
        }

        public int HighestValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int value)
        {
            this.buffer[value]++;
            if (value > this.HighestValue)
            {
                this.HighestValue = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddMultiple(int value, long count)
        {
            this.buffer[value] += count;
            if (value > this.HighestValue)
            {
                this.HighestValue = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int value)
        {
            return this.buffer[value] > 0;
        }

        public void Sort()
        {
            this.buffer.Sort();
        }
    }
}
