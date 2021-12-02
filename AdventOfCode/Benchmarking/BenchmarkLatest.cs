namespace AdventOfCode.Benchmarking
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BenchmarkDotNet.Attributes;
    using Utility;

    [MemoryDiagnoser]
    public class BenchmarkLatest
    {
        private readonly IFastDay problem;
        
        public BenchmarkLatest()
        {
            var solver = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Last(type => typeof(IFastDay).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
            
            this.problem = (IFastDay)Activator.CreateInstance(solver)!;
        }
        
        [Benchmark]
        public void Part1()
        {
            this.problem.NonReturnSolve1();
        }
        
        [Benchmark]
        public void Part2()
        {
            this.problem.NonReturnSolve2();
        }
    }
}