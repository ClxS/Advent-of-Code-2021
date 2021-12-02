namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using AoCHelper;
    using Utility;
    using Kvp = System.Collections.Generic.KeyValuePair<string, System.Action>;

    public class Day02 : BaseDay
    {
        private readonly string input;

        public Day02()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            IntVector2 position = new(0, 0);
            foreach (var line in this.input.SplitLines())
            {
                SpanStringReader reader = new(line);
                var direction = reader.ReadWord();
                var magnitude = reader.ReadInt();

                direction.Match(
                    () => throw new ArgumentOutOfRangeException(nameof(direction)),
                    new Kvp("forward", () => position += (magnitude, 0)),
                    new Kvp("up", () => position += (0, -magnitude)),
                    new Kvp("down", () => position += (0, magnitude))
                );
            }

            return new((position.X * position.Y).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            IntVector2 position = new(0, 0);
            IntVector2 aim = new(0, 0);
            foreach (var line in this.input.SplitLines())
            {
                SpanStringReader reader = new(line);
                var direction = reader.ReadWord();
                var magnitude = reader.ReadInt();

                direction.Match(
                    () => throw new ArgumentOutOfRangeException(nameof(direction)),
                    new Kvp("forward", () =>
                    {
                        position += (magnitude, magnitude * aim.Y);
                    }),
                    new Kvp("up", () => aim += (0, -magnitude)),
                    new Kvp("down", () => aim += (0, magnitude))
                );
            }

            return new((position.X * position.Y).ToString());
        }
    }
}