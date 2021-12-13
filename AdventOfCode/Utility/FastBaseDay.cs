namespace AdventOfCode.Utility
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AoCHelper;

    public abstract class FastBaseDay<T> : BaseDay, IFastDay
    {
        private string input1;
        private string input2;
        private string input;
        
        public FastBaseDay()
        {
            this.input1 = File.ReadAllText(this.InputFilePath).Replace("\r", "");
            this.input2 = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected string InputString => this.input;

        protected ReadOnlySpan<char> Input => this.input;

        public override ValueTask<string> Solve_1()
        {
            this.input = this.input1;
            var result = this.Solve1();
            return new(result?.ToString() ?? "");
        }

        public override ValueTask<string> Solve_2()
        {
            this.input = this.input2;
            var result = this.Solve2();
            return new(result?.ToString() ?? "");
        }

        public void NonReturnSolve1()
        {
            this.input = this.input1;
            this.Solve1();
        }

        public void NonReturnSolve2()
        {
            this.input = this.input2;
            this.Solve2();
        }

        protected abstract T Solve1();
        
        protected abstract T Solve2();
    }
}