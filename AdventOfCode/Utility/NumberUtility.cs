namespace AdventOfCode.Utility
{
    public class NumberUtility
    {
        public static int TriangleNumber(int number)
        {
            return (number * (number + 1)) / 2;
        }
    }
}