using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpPuzzler.TicTacToe
{
    class TicTacToeSolver
    {
        private TicTacToeBoard Board { get; }

        public TicTacToeSolver(TicTacToeBoard board)
        {
            this.Board = board;
        }

        public int Solve(string player)
        {
            TicTacToeCell c = this.GetBestCell(player);
            return c != null ? c.XIndex + this.Board.Size * c.YIndex : -1;

        }

        private List<TicTacToeLine> GetWinnableLinesForPlayer(List<TicTacToeLine> lines, string player)
        {
            return lines.Where(x => x.CanBeWonByPlayer(player)).ToList();
        }

        private TicTacToeCell GetBestCell(string player)
        {
            var lineList = this.Board.GetAllLines();

            foreach (TicTacToeLine line in lineList)
            {
                if (line.IsWinnableByPlayer(player))
                {
                    line.Cells.ForEach(x => ++x.WinningLineCount);
                }
            }

            foreach(TicTacToeCell cell in this.Board.Cells)
            {
                cell.AdjacentCellCount += Board.Cells.Count(x => (x.XIndex == cell.XIndex || x.YIndex == cell.YIndex) && x.Value != null);
            }

            var winnableLines = GetWinnableLinesForPlayer(lineList, "X");
            if (winnableLines.Count > 0)
            {
                Console.WriteLine("going for win");
                return winnableLines[0].GetFirstFreeCell();
            }

            var loseableLines = GetWinnableLinesForPlayer(lineList, "O");
            if (loseableLines.Count > 0)
            {
                Console.WriteLine("preventing loss");
                return loseableLines[0].GetFirstFreeCell();
            }

            return this.Board.Cells.Where(x => x.Value == null)
                .OrderByDescending(x => x.AdjacentCellCount)
                .OrderByDescending(x => x.WinningLineCount)
                .First();
        }

    }
}
