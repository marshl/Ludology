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

        public void Solve()
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

            var winnableLines = GetWinnableLinesForPlayer(lineList, 'X', this.Board.Columns.Count);
            if ( winnableLines.Count > 0 )
            {
                Console.WriteLine("going for win");
            }

            var loseableLines = GetWinnableLinesForPlayer(lineList, 'O', this.Board.Columns.Count);
            if (loseableLines.Count > 0)
            {
                Console.WriteLine("preventing loss");
            }

        }

        private List<TicTacToeBoard.TicTacToeLine> GetWinnableLinesForPlayer(List<TicTacToeBoard.TicTacToeLine> lines, char player, int size)
        {
            return lines.Where(x => x.Cells.Where(c => c.Value == player).Count() == size - 1 && x.Cells.Where(c => c.Value == null).Count() == 1).ToList();
        }
    }
}
