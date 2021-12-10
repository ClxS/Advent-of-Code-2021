namespace AdventOfCode.Utility
{
    using System.IO;
    using System.Threading.Tasks;
    using AoCHelper;

    public abstract class FastBaseDay<T> : BaseDay, IFastDay
    {
        public FastBaseDay()
        {
            this.Input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected string Input { get; }

        public override ValueTask<string> Solve_1()
        {
            var result = this.Solve1();
            return new(result?.ToString() ?? "");
        }

        public override ValueTask<string> Solve_2()
        {
            var result = this.Solve2();
            return new(result?.ToString() ?? "");
        }

        public void NonReturnSolve1()
        {
            this.Solve1();
        }

        public void NonReturnSolve2()
        {
            this.Solve2();
        }

        protected abstract T Solve1();
        
        protected abstract T Solve2();
    }
}