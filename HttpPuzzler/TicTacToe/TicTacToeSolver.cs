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
            Debug.Assert(this.Board.Columns.Count > 0);
            Debug.Assert(this.Board.Columns.Count == this.Board[0].Cells.Count);
            var v = this.Board.GetAllLines();
        }

        public int Solve(string player)
        {
            TicTacToeCell c = this.GetBestCell(player);
            return c.XIndex + this.Board.Size * c.YIndex;

        }

        private List<TicTacToeLine> GetWinnableLinesForPlayer(List<TicTacToeLine> lines, string player)
        {
            return lines.Where(x => x.CanBeWonByPlayer(player)).ToList();
        }

        private TicTacToeCell GetBestCell(string player)
        { 
            /*if (Board.GetCellsByPlayer('X').Count == 0)
            {
                this.PickCornerCell();
            }

            if winnable then win

            if opponent can win then prevent

                pick a spot that has the most possibilities of winning (count the number of wins that spot could help with and find the highest)

            if no cell, pick corner at random

            else choose cell that blocks most opponents moves */

            var lineList = this.Board.GetAllLines();

            foreach (TicTacToeLine line in lineList)
            {
                if (line.CanBeWonByPlayer(player))
                {
                    line.Cells.ForEach(x => ++x.WinningLineCount);
                }
            }

            var winnableLines = GetWinnableLinesForPlayer(lineList, "X");
            if (winnableLines.Count > 0)
            {
                Console.WriteLine("going for win");
                return winnableLines[0].GetFirstFreeCell();
                //winnableLines.Max( x=> x.)
            }

            var loseableLines = GetWinnableLinesForPlayer(lineList, "O");
            if (loseableLines.Count > 0)
            {
                Console.WriteLine("preventing loss");
                return loseableLines[0].GetFirstFreeCell();
            }

            return null;
        }

    }
}
