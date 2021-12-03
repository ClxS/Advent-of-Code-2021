namespace AdventOfCode.Utility
{
    public static class MaskUtil
    {
        public static int SetOnes(int length)
        {
            var output = 0;
            for (var i = 0; i < length; ++i)
            {
                output |= 1 << i;
            }
            return output;
        }
    }
}