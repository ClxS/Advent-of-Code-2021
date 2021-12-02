namespace AdventOfCode.Utility
{
    using System;
    using System.Runtime.CompilerServices;

    public ref struct SpanStringReader
    {
        private ReadOnlySpan<char> data;

        public SpanStringReader(ReadOnlySpan<char> sourceString)
        {
            this.data = sourceString;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> ReadLine()
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var idx = 0;
            while(idx < this.data.Length)
            {
                if (idx + 1 >= this.data.Length || this.data[idx + 1] == '\n')
                {
                    break;
                }

                idx++;
            }

            var skip = 1;
            if (this.data[idx] == '\r')
            {
                skip++;
            }

            if (idx > 0 && this.data[idx - 1] == '\r')
            {
                idx--;
            }

            if (idx + 1 == this.data.Length)
            {
                idx++;
            }

            var retValue = this.data.Slice(0, idx);
            this.data = this.data[(Math.Min(idx + skip, this.data.Length))..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> ReadUntil(char v, bool skipFound)
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var idx = 0;
            while (idx < this.data.Length)
            {
                if (this.data[idx] == v)
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data.Slice(0, idx);

            if (skipFound && idx < this.data.Length - 1)
            {
                idx++;
            }

            this.data = this.data[idx..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> ReadUntilDigit(bool skipFound)
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var idx = 0;
            while (idx < this.data.Length)
            {
                if (this.data[idx] >= '0' && this.data[idx] <= '9')
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data.Slice(0, idx);

            if (skipFound && idx < this.data.Length - 1)
            {
                idx++;
            }

            this.data = this.data[idx..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int count)
        {
            this.data = this.data[count..];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEndOfFile()
        {
            return this.data.Length == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> ReadWord(bool skipToNextChar = true)
        {
            var dataLength = this.data.Length;
            if (dataLength == 0)
            {
                return default;
            }

            var idx = 0;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (@char is not (>= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9'))
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data[..(idx + 1)];

            if (skipToNextChar)
            {
                this.ProceedToNextChar(ref idx);
            }

            this.data = this.data[(idx + 1)..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> ReadSymbol(bool skipToNextChar = true)
        {
            var dataLength = this.data.Length;
            if (dataLength == 0)
            {
                return default;
            }

            var idx = 0;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (@char == ' ' || @char == '\r')
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data.Slice(0, idx + 1);

            if (skipToNextChar)
            {
                this.ProceedToNextChar(ref idx);
            }

            this.data = this.data[(idx + 1)..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> PeekWord()
        {
            var dataLength = this.data.Length;
            if (dataLength == 0)
            {
                return default;
            }

            var idx = 0;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (@char is not (>= 'a' and <= 'z' or >= '0' and <= '9'))
                {
                    break;
                }

                idx++;
            }

            return this.data[..(idx + 1)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ReadChar(bool skipToNextChar = true)
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var idx = 0;
            var retValue = this.data[0];

            if (skipToNextChar)
            {
                this.ProceedToNextChar(ref idx);
            }

            this.data = this.data[(idx + 1)..];
            return retValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(bool skipToNextChar = true)
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var dataLength = this.data.Length;
            var idx = 0;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (@char is not (>= '0' and <= '9'))
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data.Slice(0, idx + 1);

            if (skipToNextChar)
            {
                this.ProceedToNextChar(ref idx);
            }

            this.data = this.data[(idx + 1)..];
            return NumberParser.ParseInt(retValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUlongBase2(bool skipToNextChar = true)
        {
            if (this.data.Length == 0)
            {
                return default;
            }

            var dataLength = this.data.Length;
            var idx = 0;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (@char is not (>= '0' and <= '1'))
                {
                    break;
                }

                idx++;
            }

            var retValue = this.data[..(idx + 1)];

            if (skipToNextChar)
            {
                this.ProceedToNextChar(ref idx);
            }

            this.data = this.data[(idx + 1)..];
            return NumberParser.ParseUlongBase2(retValue);
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProceedToNextChar(ref int idx)
        {
            var dataLength = this.data.Length;
            while (idx < this.data.Length && idx + 1 < dataLength)
            {
                var @char = this.data[idx + 1];
                if (!(@char == '\r' || @char == '\n' || @char == ' '))
                {
                    break;
                }

                idx++;
            }
        }
    }
}
