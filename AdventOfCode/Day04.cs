namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Toolkit.HighPerformance;
    using Utility;

    // ReSharper disable once UnusedType.Global
    public sealed class Day04 : FastBaseDay<int>
    {
        private readonly string input;

        public Day04()
        {
            this.input = File.ReadAllText(this.InputFilePath).Replace("\r", "");
        }

        protected override int Solve1()
        {
            this.CreateInitialData(out var data, out var numbersLine, out var boards);
            this.ReadBoardData(boards, data);
            
            var numbersReader = new SpanStringReader(numbersLine);
            while (true)
            {
                var number = numbersReader.ReadInt();
                foreach (var board in boards)
                {
                    if (board.CheckForCompleteWithNumber(number))
                    {
                        return board.SumUnmarked() * number;
                    }
                }
                
                numbersReader.Skip(1);
            }
        }

        protected override int Solve2()
        {
            this.CreateInitialData(out var data, out var numbersLine, out var boards);
            this.ReadBoardData(boards, data);
            
            var numbersReader = new SpanStringReader(numbersLine);
            var boardsRemaining = boards.Length;
            while (true)
            {
                var number = numbersReader.ReadInt();
                if (number == 15)
                {
                }
                
                foreach (var board in boards)
                {
                    if (board.IsComplete)
                    {
                        continue;
                    }
                    
                    if (board.CheckForCompleteWithNumber(number))
                    {
                        boardsRemaining--;
                        if (boardsRemaining == 0)
                        {
                            return board.SumUnmarked() * number;
                        }
                    }
                }
                
                numbersReader.Skip(1);
            }
        }

        private void CreateInitialData(out ReadOnlySpan<char> data, out ReadOnlySpan<char> numbers, out Board5x5[] boards)
        {
            var currentIdx = this.input.IndexOf('\n');
            numbers = this.input.AsSpan()[0..currentIdx];
            data = this.input.AsSpan()[(currentIdx + 2)..];
            var blockCount = data.CountCharacters('\n') / 6 + 1;
            boards = new Board5x5[blockCount];
        }

        private void ReadBoardData(Board5x5[] boards, ReadOnlySpan<char> data)
        {
            var boardIdx = 0;
            while (true)
            {
                var blockEnd = data.IndexOf("\n\n");
                if (blockEnd == -1)
                {
                    this.ReadBoard(ref boards[boardIdx++], data);
                    break;
                }

                this.ReadBoard(ref boards[boardIdx++], data[..blockEnd]);
                data = data[(blockEnd + 2)..];
            }
        }

        private void ReadBoard(ref Board5x5 board, ReadOnlySpan<char> boardText)
        {
            var rowIdx = 0;
            var data = new int[5, 5];
            foreach (var row in boardText.SplitLines())
            {
                var reader = new SpanStringReader(row);
                reader.SkipSpace();
                data[rowIdx, 0] = reader.ReadInt();
                data[rowIdx, 1] = reader.ReadInt();
                data[rowIdx, 2] = reader.ReadInt();
                data[rowIdx, 3] = reader.ReadInt();
                data[rowIdx++, 4] = reader.ReadInt();
            }

            board = new()
            {
                Data = data
            };
        }

        private class Board5x5
        {
            public int[,] Data;

            public bool IsComplete { get; private set; }

            public bool CheckForCompleteWithNumber(int number)
            {
                for (var y = 0; y < this.Data.GetLength(0); y++)
                {
                    for (var x = 0; x < this.Data.GetLength(1); x++)
                    {
                        if (this.Data[y, x] != number)
                        {
                            continue;
                        }

                        this.Data[y, x] = -1;
                        if (this.IsRowOrColumnComplete(x, y))
                        {
                            this.IsComplete = true;
                            return true;
                        }

                        return false;
                    }
                }

                return false;
            }

            private bool IsRowOrColumnComplete(int column, int row)
            {
                var isComplete = true;
                for (var i = 0; i < 5; i++)
                {
                    if (this.Data[i, column] != -1)
                    {
                        isComplete = false;
                        break;
                    }
                }

                if (isComplete)
                {
                    return true;
                }

                isComplete = true;
                for (var i = 0; i < 5; i++)
                {
                    if (this.Data[row, i] != -1)
                    {
                        isComplete = false;
                        break;
                    }
                }

                return isComplete;
            }

            public int SumUnmarked()
            {
                var sum = 0;
                foreach (var cell in this.Data)
                {
                    if (cell != -1)
                    {
                        sum += cell;
                    }
                }

                return sum;
            }
        }
    }
}