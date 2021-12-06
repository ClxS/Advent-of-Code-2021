namespace AdventOfCode.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class StringExtensions
    {
        private static char[] lines = { '\r', '\n' };

        public static int CountCharacters(this ReadOnlySpan<char> str, char character)
        {
            var count = 0;
            foreach (var c in str)
            {
                if (c == character)
                {
                    count++;
                }
            }

            return count;
        }
        
        public static SplitEnumerator SplitLines(this string str)
        {
            return str.SplitAsSpans(lines);
        }

        public static SplitEnumerator SplitLines(this ReadOnlySpan<char> str)
        {
            return str.SplitAsSpans(lines);
        }
        
        public static SplitEnumerator SplitAsSpans(this string str, ReadOnlySpan<char> separators)
        {
            return new(str.AsSpan(), separators);
        }

        public static SplitEnumerator SplitAsSpans(this ReadOnlySpan<char> str, ReadOnlySpan<char> separators)
        {
            return new(str, separators);
        }

        public static SplitEnumeratorAsMemory SplitLinesAsMemory(this string str)
        {
            return str.SplitAsSpansAsMemory(new[] { '\r', '\n' });
        }

        public static SplitEnumeratorAsMemory SplitAsSpansAsMemory(this string str, ReadOnlySpan<char> separators)
        {
            return new(MemoryExtensions.AsMemory(str), separators);
        }

        public static SplitEnumeratorAsMemory SplitAsSpansAsMemory(this ReadOnlyMemory<char> str, ReadOnlySpan<char> separators)
        {
            return new(str, separators);
        }

        public ref struct SplitEnumerator
        {
            private SplitEntry current;
            private ReadOnlySpan<char> chars;
            private readonly ReadOnlySpan<char> separators;

            public SplitEnumerator(ReadOnlySpan<char> str, ReadOnlySpan<char> separators)
            {
                this.chars = str;
                this.separators = separators;
                this.current = default;
            }

            // Needed to be compatible with the foreach operator
            public SplitEnumerator GetEnumerator() => this;

            public bool MoveNext()
            {
                var span = this.chars;
                if (span.Length == 0) // Reach the end of the string
                    return false;

                var index = span.IndexOfAny(this.separators);
                if (index == -1) // The string is composed of only one line
                {
                    this.chars = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
                    this.current = new(span, ReadOnlySpan<char>.Empty);
                    return true;
                }

                if (index < span.Length - 1)
                {
                    // Try to consume the '\n' associated to the '\r'
                    var next = span[index + 1];
                    if (this.separators.Contains(next))
                    {
                        this.current = new(span[..index], span.Slice(index, 2));
                        this.chars = span[(index + 2)..];
                        return true;
                    }
                }

                this.current = new(span[..index], span.Slice(index, 1));
                this.chars = span[(index + 1)..];
                return true;
            }

            public ReadOnlySpan<char> Current => this.current.Line;
        }

        public ref struct SplitEnumeratorAsMemory
        {
            private SplitEntryAsMemory current;
            private ReadOnlyMemory<char> chars;
            private readonly ReadOnlySpan<char> separators;

            public SplitEnumeratorAsMemory(ReadOnlyMemory<char> str, ReadOnlySpan<char> separators)
            {
                this.chars = str;
                this.separators = separators;
                this.current = default;
            }

            // Needed to be compatible with the foreach operator
            public SplitEnumeratorAsMemory GetEnumerator() => this;

            public bool MoveNext()
            {
                var span = this.chars;
                if (span.Length == 0) // Reach the end of the string
                    return false;

                var index = span.Span.IndexOfAny(this.separators);
                if (index == -1) // The string is composed of only one line
                {
                    this.chars = ReadOnlyMemory<char>.Empty; // The remaining string is an empty string
                    this.current = new(span, ReadOnlyMemory<char>.Empty);
                    return true;
                }

                if (index < span.Length - 1)
                {
                    // Try to consume the '\n' associated to the '\r'
                    var next = span.Span[index + 1];
                    if (this.separators.Contains(next))
                    {
                        this.current = new(span.Slice(0, index), span.Slice(index, 2));
                        this.chars = span[(index + 2)..];
                        return true;
                    }
                }

                this.current = new(span.Slice(0, index), span.Slice(index, 1));
                this.chars = span[(index + 1)..];
                return true;
            }

            public ReadOnlyMemory<char> Current => this.current.Line;
        }

        public readonly ref struct SplitEntry
        {
            public SplitEntry(ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
            {
                this.Line = line;
                this.Separator = separator;
            }

            public ReadOnlySpan<char> Line { get; }

            public ReadOnlySpan<char> Separator { get; }

            // https://docs.microsoft.com/en-us/dotnet/csharp/deconstruct#deconstructing-user-defined-types
            public void Deconstruct(out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
            {
                line = this.Line;
                separator = this.Separator;
            }

            // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
            // foreach (ReadOnlySpan<char> entry in str.SplitLines())
            public static implicit operator ReadOnlySpan<char>(SplitEntry entry) => entry.Line;
        }

        public readonly ref struct SplitEntryAsMemory
        {
            public SplitEntryAsMemory(ReadOnlyMemory<char> line, ReadOnlyMemory<char> separator)
            {
                this.Line = line;
                this.Separator = separator;
            }

            public ReadOnlyMemory<char> Line { get; }

            public ReadOnlyMemory<char> Separator { get; }

            // https://docs.microsoft.com/en-us/dotnet/csharp/deconstruct#deconstructing-user-defined-types
            public void Deconstruct(out ReadOnlyMemory<char> line, out ReadOnlyMemory<char> separator)
            {
                line = this.Line;
                separator = this.Separator;
            }

            // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
            // foreach (ReadOnlySpan<char> entry in str.SplitLines())
            public static implicit operator ReadOnlyMemory<char>(SplitEntryAsMemory entry) => entry.Line;
        }

        

        public static unsafe void Match<TContext>(
            this ReadOnlySpan<char> str,
            WrappedPointer<TContext> unmatched, 
            KeyValuePair<string, WrappedPointer<TContext>> actions1, 
            ref TContext context)
        {
            if (str.SequenceEqual(actions1.Key))
            {
                actions1.Value.Delegate(ref context);
            }
            else
            {
                unmatched.Delegate(ref context);
            }
        }
        
        public static unsafe void Match<TConect>(
            this ReadOnlySpan<char> str,
            WrappedPointer<TConect> unmatched, 
            KeyValuePair<string, WrappedPointer<TConect>> actions1, 
            KeyValuePair<string, WrappedPointer<TConect>> actions2,
            ref TConect context)
        {
            if (str.SequenceEqual(actions1.Key))
            {
                actions1.Value.Delegate(ref context);
            }
            else if (str.SequenceEqual(actions2.Key))
            {
                actions2.Value.Delegate(ref context);
            }
            else
            {
                unmatched.Delegate(ref context);
            }
        }
        public static unsafe void Match<T>(
            this ReadOnlySpan<char> str,
            ref T context,
            WrappedPointer<T> unmatched, 
            KeyValuePair<string, WrappedPointer<T>> actions1, 
            KeyValuePair<string, WrappedPointer<T>> actions2, 
            KeyValuePair<string, WrappedPointer<T>> actions3)
        {
            if (str.SequenceEqual(actions1.Key))
            {
                actions1.Value.Delegate(ref context);
            }
            else if (str.SequenceEqual(actions2.Key))
            {
                actions2.Value.Delegate(ref context);
            }
            else if (str.SequenceEqual(actions3.Key))
            {
                actions3.Value.Delegate(ref context);
            }
            else
            {
                unmatched.Delegate(ref context);
            }
        }
    }
}
