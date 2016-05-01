//-----------------------------------------------------------------------
// <copyright file="TicTacToeSolver.cs" company="marshl">
// Copyright 2016, Liam Marshall, marshl.
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------
namespace Ludology.TicTacToe
{
    using System.Linq;

    /// <summary>
    /// Used to solve a TicTacToeBoard
    /// </summary>
    public class TicTacToeSolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeSolver"/> class.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        public TicTacToeSolver(TicTacToeBoard board)
        {
            this.Board = board;
        }

        /// <summary>
        /// Gets the board to solve.
        /// </summary>
        private TicTacToeBoard Board { get; }

        /// <summary>
        /// Finds the best move for the current board.
        /// </summary>
        /// <param name="player">The player to find the move for.</param>
        /// <returns>The index to place the cell.</returns>
        public int Solve(string player)
        {
            TicTacToeCell c = this.GetBestCell(player);
            return c != null ? c.XIndex + (c.YIndex * this.Board.Size) : -1;
        }

        /// <summary>
        /// Gets the best cell for the given player using the current board state.
        /// </summary>
        /// <param name="player">The player to find the best move for.</param>
        /// <returns>The cell that is best for the player.</returns>
        private TicTacToeCell GetBestCell(string player)
        {
            // If no cells have been marked yet, then pick the top left corner
            if (this.Board.Cells.Count(x => x.Mark != null) == 0)
            {
                return this.Board.Cells[0];
            }

            var lineList = this.Board.GetAllLines();

            foreach (TicTacToeLine line in lineList.Where(x => x.IsWinnableByPlayer(player)))
            {
                line.Cells.ForEach(x => ++x.WinningLineCount);
            }

            // Finds the adjacent cell count for all cells
            foreach (TicTacToeCell cell in this.Board.Cells)
            {
                cell.AdjacentCellCount += this.Board.Cells.Count(x => (x.XIndex == cell.XIndex || x.YIndex == cell.YIndex) && x.Mark != null);
            }

            // If there is a spot that can be won, then go for it
            var winnableLines = lineList.Where(x => x.CanBeWonByPlayer(player)).ToArray();
            if (winnableLines.Length > 0)
            {
                return winnableLines[0].GetFirstUnmarkedCell();
            }

            // If there is a cell where the opponent can win, the stop them
            var loseableLines = lineList.Where(x => x.CanBeWonByPlayer(player == "X" ? "O" : "X")).ToArray();
            if (loseableLines.Length > 0)
            {
                return loseableLines[0].GetFirstUnmarkedCell();
            }

            // Otherwise pick the first empty cell, prefering those with a high number of lines, followed by a high number of adjacent cells
            return this.Board.Cells.Where(x => x.Mark == null)
                .OrderByDescending(x => x.AdjacentCellCount)
                .OrderByDescending(x => x.WinningLineCount)
                .First();
        }
    }
}
