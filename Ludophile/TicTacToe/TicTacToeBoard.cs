//-----------------------------------------------------------------------
// <copyright file="TicTacToeBoard.cs" company="marshl">
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
namespace Ludophile.TicTacToe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The game state of a tic-tac-toe game
    /// </summary>
    public class TicTacToeBoard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeBoard"/> class.
        /// </summary>
        /// <param name="gameState">The array of game state to initialize with.</param>
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

            // Create a new TicTacToeCell for each element in the gamestate
            this.Cells = gameState.Select((x, i) => new TicTacToeCell(i % this.Size, i / this.Size, x)).ToArray();
        }

        /// <summary>
        /// Gets the height and width of the board
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the table of cells as a flat array.
        /// </summary>
        public TicTacToeCell[] Cells { get; }

        /// <summary>
        /// Gets the cells owned by the given player.
        /// </summary>
        /// <param name="player">The player to search for.</param>
        /// <returns>The list of cells owned by the given player.</returns>
        public List<TicTacToeCell> GetCellsByPlayer(string player)
        {
            return this.Cells.Where(x => x.Mark == player).ToList();
        }

        /// <summary>
        /// Gets the cell at the given position.
        /// </summary>
        /// <param name="x">The x index of the cell.</param>
        /// <param name="y">The y index of the cell.</param>
        /// <returns>The cell at the given position.</returns>
        public TicTacToeCell GetCell(int x, int y)
        {
            if (x < 0 || x > this.Size || y < 0 || y > this.Size)
            {
                throw new IndexOutOfRangeException();
            }

            return this.Cells[x + (y * this.Size)];
        }

        /// <summary>
        /// Gets all lines in the board.
        /// </summary>
        /// <returns>The list of lines</returns>
        public List<TicTacToeLine> GetAllLines()
        {
            var diagonalLines = new List<TicTacToeLine>();
            diagonalLines.Add(new TicTacToeLine());
            diagonalLines.Add(new TicTacToeLine());

            // Find the two diagonal lines
            for (int i = 0; i < this.Size; ++i)
            {
                diagonalLines[0].Cells.Add(this.GetCell(i, i));
                diagonalLines[1].Cells.Add(this.GetCell(i, this.Size - i - 1));
            }

            // Find the row and column lines
            var columnLines = new List<TicTacToeLine>();
            var rowLines = new List<TicTacToeLine>();
            for (int x = 0; x < this.Size; ++x)
            {
                columnLines.Add(new TicTacToeLine());
                rowLines.Add(new TicTacToeLine());
            }

            for (int x = 0; x < this.Size; ++x)
            {
                for (int y = 0; y < this.Size; ++y)
                {
                    columnLines[x].Cells.Add(this.GetCell(x, y));
                    rowLines[y].Cells.Add(this.GetCell(x, y));
                }
            }

            // Combine the lists and return them
            columnLines.AddRange(rowLines);
            columnLines.AddRange(diagonalLines);
            return columnLines;
        }
    }
}
