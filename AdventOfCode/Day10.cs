namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day10 : FastBaseDay<ulong>
    {
        protected override ulong Solve1()
        {
            var buf = new StackCircularBuffer<char>(stackalloc char[512]);
            var errors = 0ul;
            foreach (var line in this.Input.EnumerateLines())
            {
                var stop = false;
                buf.Clear();
                foreach (var @char in line)
                {
                    switch (@char)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            buf.PushBack(@char);
                            break;
                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            // 2 chars diff between the start and ends
                            if (buf.Back() != @char - (@char == ')' ? 1 : 2))
                            {
                                errors += @char switch
                                {
                                    ')' => 3ul,
                                    ']' => 57ul,
                                    '}' => 1197ul,
                                    '>' => 25137ul,
                                };
                                stop = true;
                            }
                            else
                            {
                                buf.PopBack();
                            }
                            break;
                    }

                    if (stop)
                    {
                        break;
                    }
                }
            }
            
            return errors;
        }

        protected override ulong Solve2()
        {
            var buf = new StackCircularBuffer<char>(stackalloc char[512]);
            var costs = new List<ulong>();
            foreach (var line in this.Input.EnumerateLines())
            {
                var fixupCost = 0ul;
                var stop = false;
                buf.Clear();
                foreach (var @char in line)
                {
                    switch (@char)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            buf.PushBack(@char);
                            break;
                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            // 2 chars diff between the start and ends
                            if (buf.Back() != @char - (@char == ')' ? 1 : 2))
                            {
                                stop = true;
                            }
                            else
                            {
                                buf.PopBack();
                            }
                            break;
                    }

                    if (stop)
                    {
                        break;
                    }
                }

                if (stop)
                {
                    continue;
                }

                while (!buf.IsEmpty)
                {
                    var neededChar = @buf.PopBack();
                    var extraCost = (ulong)(neededChar switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                    });
                        
                    fixupCost = fixupCost * 5ul + extraCost;
                }
                
                costs.Add(fixupCost);
            }
            
            costs.Sort();
            return costs[costs.Count / 2];
        }
    }
}