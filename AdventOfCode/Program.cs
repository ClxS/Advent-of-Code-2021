using System;
using System.Linq;
using AdventOfCode.Benchmarking;
using AoCHelper;
using BenchmarkDotNet.Running;

if (args.Length == 0)
{
    Solver.SolveLast();
    #if !DEBUG
    BenchmarkRunner.Run<BenchmarkLatest>();
    #endif
}
else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
{
    Solver.SolveAll();
}
else
{
    var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);

    Solver.Solve(indexes.Where(i => i < uint.MaxValue));
}