namespace AdventOfCode.Utility
{
    using System;

    public struct IntVector2
    {
        public IntVector2(int x, int y) : this()
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator IntVector2(ValueTuple<int, int> input)
        {
            return new IntVector2(input.Item1, input.Item2);
        }

        public int X { get; set; }

        public int Y { get; set; }
        
        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public static IntVector2 operator -(IntVector2 a, IntVector2 b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public static IntVector2 operator -(IntVector2 a)
        {
            return new(-a.X, -a.Y);
        }

        public static IntVector2 operator /(IntVector2 a, int multiplier)
        {
            return new(a.X / multiplier, a.Y / multiplier);
        }

        public static IntVector2 operator /(IntVector2 a, double multiplier)
        {
            return new((int)(a.X / multiplier), (int)(a.Y / multiplier));
        }

        public static IntVector2 operator *(IntVector2 a, IntVector2 b)
        {
            return new(a.X * b.X, a.Y * b.Y);
        }

        public static IntVector2 operator *(IntVector2 a, int multiplier)
        {
            return new(a.X * multiplier, a.Y * multiplier);
        }

        public static IntVector2 operator *(IntVector2 a, double multiplier)
        {
            return new((int)(a.X * multiplier), (int)(a.Y * multiplier));
        }

        public override string ToString()
        {
            return $"{this.X}, {this.Y}";
        }
    }
}