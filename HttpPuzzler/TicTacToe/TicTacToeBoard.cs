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
            this.Size = (int)Math.Floor(Math.Sqrt(gameState.Length));
            if (gameState.Length % this.Size != 0)
            {
                throw new ArgumentException("Board must be square.");
            }

            for (int x = 0; x < this.Size; ++x)
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
            }
        }

        public List<TicTacToeColumn> Columns { get; set; }

        public TicTacToeColumn this[int index]
        {
            get
            {
                return this.Columns[index];
            }
        }

        public List<TicTacToeCell> GetCellsByPlayer(char player)
        {
            var cells = new List<TicTacToeCell>();
            this.Columns.ForEach(
                x => x.Cells.ForEach(
                    y => { if (y.Value == player) cells.Add(y); }
                )
            );

            return cells;
        }

        public List<TicTacToeLine> GetAllLines()
        {
            Debug.Assert(this.Columns.Count > 0);
            Debug.Assert(this.Columns.Count == this.Columns[0].Cells.Count);

            int scale = this.Columns.Count;

            var columnLines = new List<TicTacToeLine>();
            var rowLines = new List<TicTacToeLine>();
            var diagonalLines = new List<TicTacToeLine>();
            diagonalLines.Add(new TicTacToeLine());
            diagonalLines.Add(new TicTacToeLine());

            for (int i = 0; i < this.Columns.Count; ++i)
            {
                diagonalLines[0].Cells.Add(this[i][i]);
                diagonalLines[1].Cells.Add(this[i][scale - i - 1]);
            }

            for (int x = 0; x < scale; ++x)
            {
                columnLines.Add(new TicTacToeLine());
                rowLines.Add(new TicTacToeLine());
            }
            for (int x = 0; x < scale; ++x)
            { 
                for (int y = 0; y < scale; ++y)
                {
                    columnLines[x].Cells.Add(this[x][y]);
                    rowLines[y].Cells.Add(this[x][y]);
                }
            }

            columnLines.AddRange(rowLines);
            columnLines.AddRange(diagonalLines);
            return columnLines;
        }

       
    }

    public class TicTacToeColumn
    {
        public List<TicTacToeCell> Cells { get; set; }

        public TicTacToeCell this[int index]
        {
            get
            {
                return this.Cells[index];
            }
        }
    }

    public class TicTacToeCell
    {
        public char? Value { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }

    public class TicTacToeLine
    {
        public List<TicTacToeCell> Cells = new List<TicTacToeCell>();

        public bool CanBeWonByPlayer(char player)
        {
            return this.Cells.Where(c => c.Value == player).Count() == this.Cells.Count - 1
                && this.Cells.Where(c => c.Value == null).Count() == 1;
        }

        public bool IsWinnableByPlayer(char player)
        {
            return this.Cells.Where(c => (c.Value ?? player) != player).Count() == 0;
        }
    }
}
