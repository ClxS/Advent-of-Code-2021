namespace AdventOfCode.Utility
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class FileUtil
    {
        public static IEnumerable<int> GetIntArray(string filePath)
        {
            List<int> numbers = new();
            foreach (var line in File.ReadAllText(filePath).SplitLines())
            {
                numbers.Add(NumberParser.ParseInt(line));
            }

            return numbers;
        }
    }
}
