namespace AdventOfCode.Utility
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AoCHelper;

    public abstract class FastBaseDay<T> : BaseDay, IFastDay
    {
        private string input;
        
        public FastBaseDay()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected ReadOnlySpan<char> Input => this.input;

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