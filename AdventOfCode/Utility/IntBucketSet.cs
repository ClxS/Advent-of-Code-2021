namespace AdventOfCode.Utility
{
    using System.Collections.Generic;

    public class IntBucketSet
    {
        private readonly byte[] hashTable;
        private readonly int minOffset;

        public IntBucketSet(IList<int> data)
        {
            var max = 0;
            var min = 0;
            foreach (var t in data)
            {
                if (t > max)
                {
                    max = t;
                }

                if (t < min)
                {
                    min = t;
                }
            }

            this.hashTable = new byte[(max - min) + 1];
            this.minOffset = -min;

            foreach (var t in data)
            {
                this.hashTable[t] = 1;
            }
        }

        public bool Contains(int value)
        {
            if (value < this.minOffset || value > this.minOffset + value)
            {
                return false;
            }

            return this.hashTable[this.minOffset + value] > 0;
        }
    }
}
