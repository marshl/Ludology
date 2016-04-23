using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HttpPuzzler.TicTacToe
{
    class TicTacToeBoard
    {
        public int Size { get; }

        public TicTacToeBoard(string[] gameState)
        {
            if (gameState.Length == 0)
            {
                throw new ArgumentException("The game state must at least one element.");
            }

            this.Size = (int)Math.Floor(Math.Sqrt(gameState.Length));
            if (gameState.Length % this.Size != 0)
            {
                throw new ArgumentException("Board must be square.");
            }

            /*for (int x = 0; x < this.Size; ++x)
            {
                this.Columns.Add(new TicTacToeColumn());
                this.Columns[x].Cells = new List<TicTacToeCell>();
                for (int y = 0; y < this.Size; ++y)
                {
                    TicTacToeCell c = new TicTacToeCell();
                    c.XIndex = x;
                    c.YIndex = y;
                    c.Value = gameState[x + y * this.Size];
                    this.Columns[x].Cells.Add(c);
                }
            }*/

            this.Cells = gameState.Select((x, i) => new TicTacToeCell(i % Size, i / Size, x)).ToList();
        }

        public List<TicTacToeCell> Cells { get; set; }
        //public List<TicTacToeColumn> Columns { get; set; }

        /*public TicTacToeColumn this[int index]
        {
            get
            {
                return this.Columns[index];
            }
        }*/

        public List<TicTacToeCell> GetCellsByPlayer(string player)
        {
            var cells = new List<TicTacToeCell>();
            /*this.Columns.ForEach(
                x => x.Cells.ForEach(
                    y => { if (y.Value == player) cells.Add(y); }
                )
            );*/
            this.Cells.Where(x => x.Value == player);

            return cells;
        }

        public TicTacToeCell GetCell(int x, int y)
        {
            return this.Cells[x + this.Size * y];
        }

        public List<TicTacToeLine> GetAllLines()
        {
            /*Debug.Assert(this.Columns.Count > 0);
            Debug.Assert(this.Columns.Count == this.Columns[0].Cells.Count);
            */
            //int scale = this.Columns.Count;

            var columnLines = new List<TicTacToeLine>();
            var rowLines = new List<TicTacToeLine>();
            var diagonalLines = new List<TicTacToeLine>();
            diagonalLines.Add(new TicTacToeLine());
            diagonalLines.Add(new TicTacToeLine());

            for (int i = 0; i < this.Size; ++i)
            {
                diagonalLines[0].Cells.Add(this.GetCell(i, i));
                diagonalLines[1].Cells.Add(this.GetCell(i, this.Size - i - 1));
            }

            for (int x = 0; x < this.Size; ++x)
            {
                columnLines.Add(new TicTacToeLine());
                rowLines.Add(new TicTacToeLine());
            }
            for (int x = 0; x < this.Size; ++x)
            {
                for (int y = 0; y < this.Size; ++y)
                {
                    columnLines[x].Cells.Add(this.GetCell(x,y));
                    rowLines[y].Cells.Add(this.GetCell(x,y));
                }
            }

            columnLines.AddRange(rowLines);
            columnLines.AddRange(diagonalLines);
            return columnLines;
        }


    }

    /*public class TicTacToeColumn
    {
        public List<TicTacToeCell> Cells { get; set; }

        public TicTacToeCell this[int index]
        {
            get
            {
                return this.Cells[index];
            }
        }
    }*/

    public class TicTacToeCell
    {
        public string Value { get; }
        public int XIndex { get; }
        public int YIndex { get; }

        public int WinningLineCount { get; set; }

        public int AdjacentCellCount { get; set; }

        public TicTacToeCell(int x, int y, string value)
        {
            this.Value = value;
            this.XIndex = x;
            this.YIndex = y;
        }
    }

    public class TicTacToeLine
    {
        public List<TicTacToeCell> Cells = new List<TicTacToeCell>();

        public bool CanBeWonByPlayer(string player)
        {
            return this.Cells.Where(c => c.Value == player).Count() == this.Cells.Count - 1
                && this.Cells.Where(c => c.Value == null).Count() == 1;
        }

        public bool IsWinnableByPlayer(string player)
        {
            return this.Cells.Where(c => (c.Value ?? player) != player).Count() == 0;
        }

        public TicTacToeCell GetFirstFreeCell()
        {
            return this.Cells.First(x => x.Value == null);
        }
    }
}
