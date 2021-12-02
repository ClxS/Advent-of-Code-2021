namespace AdventOfCode
{
    using System;
    using System.IO;
    using Utility;
    using Kvp = System.Collections.Generic.KeyValuePair<string, System.Action>;

    public sealed unsafe class Day02 : FastBaseDay<Day02.MatchContext>
    {
        private readonly string input;

        public Day02()
        {
            this.input = File.ReadAllText(this.InputFilePath);
        }

        protected override MatchContext Solve1()
        {
            MatchContext context = new();
            foreach (var line in this.input.SplitLines())
            {
                SpanStringReader reader = new(line);
                var direction = reader.ReadWord();
                context.Magnitude = reader.ReadInt();

                direction.Match(
                    ref context,
                    new(&OnInvalidDirection),
                    new("forward", new(&OnForward)),
                    new("up", new(&OnUp)),
                    new("down", new(&OnDown))
                );
            }

            return context;
        }

        protected override MatchContext Solve2()
        {
            MatchContext context = new();
            foreach (var line in this.input.SplitLines())
            {
                SpanStringReader reader = new(line);
                var direction = reader.ReadWord();
                context.Magnitude = reader.ReadInt();

                direction.Match(
                    ref context,
                    new(&OnInvalidDirection),
                    new("forward", new(&OnForwardAimed)),
                    new("up", new(&OnUpAimed)),
                    new("down", new(&OnDownAimed))
                );
            }

            return context;
        }

        private static void OnInvalidDirection(ref MatchContext context)
        {
            throw new ArgumentOutOfRangeException("direction");
        }

        private static void OnForward(ref MatchContext context)
        {
            context.Position += (context.Magnitude, 0);
        }

        private static void OnUp(ref MatchContext context)
        {
            context.Position += (0, -context.Magnitude);
        }

        private static void OnDown(ref MatchContext context)
        {
            context.Position += (0, context.Magnitude);
        }

        private static void OnForwardAimed(ref MatchContext context)
        {
            context.Position += (context.Magnitude, context.Magnitude * context.Aim.Y);
        }

        private static void OnUpAimed(ref MatchContext context)
        {
            context.Aim += (0, -context.Magnitude);
        }

        private static void OnDownAimed(ref MatchContext context)
        {
            context.Aim += (0, context.Magnitude);
        }

        public struct MatchContext
        {
            public IntVector2 Position;

            public IntVector2 Aim;

            public int Magnitude;

            public override string ToString()
            {
                return (this.Position.X * this.Position.Y).ToString();
            }
        }
    }
}