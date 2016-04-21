using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpPuzzler.TicTacToe
{
    class TicTacToeBoard
    {
        public List<TicTacToeColumn> Columns { get; set; }

        public TicTacToeColumn this[int index]
        {
            get
            {
                return this.Columns[index];
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

            public class TicTacToeCell
            {
                public char? Value { get; set; }
                public int XIndex { get; set; }
                public int YIndex { get; set; }
            }
        }

        public List<TicTacToeColumn.TicTacToeCell> GetCellsByPlayer(char player)
        {
            var cells = new List<TicTacToeColumn.TicTacToeCell>();
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

        public class TicTacToeLine
        {
            public List<TicTacToeColumn.TicTacToeCell> Cells = new List<TicTacToeColumn.TicTacToeCell>();
        }
    }
}
