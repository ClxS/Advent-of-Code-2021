namespace AdventOfCode.Benchmarking
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AoCHelper;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser]
    public class BenchmarkLatest
    {
        private readonly BaseProblem problem;
        
        public BenchmarkLatest()
        {
            var solver = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Last(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
            
            this.problem = (BaseProblem)Activator.CreateInstance(solver)!;
        }
        
        [Benchmark]
        public async ValueTask<string> Part1()
        {
            return await this.problem.Solve_1();
        }
        
        [Benchmark]
        public async ValueTask<string> Part2()
        {
            return await this.problem.Solve_2();
        }
    }
}