namespace AdventOfCode
{
    using System.IO;
    using System.Threading.Tasks;
    using AoCHelper;

    public class Day01 : BaseDay
    {
        private readonly string input;

        public Day01()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            return new($"Solution to {this.ClassPrefix} {this.CalculateIndex()}, part 1");
        }

        public override ValueTask<string> Solve_2()
        {
            return new($"Solution to {this.ClassPrefix} {this.CalculateIndex()}, part 2");
        }
    }
}