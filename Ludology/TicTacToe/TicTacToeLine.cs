//-----------------------------------------------------------------------
// <copyright file="TicTacToeLine.cs" company="marshl">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A contiguous line of TicTacToeCells on a TicTacToeBoard
    /// </summary>
    public class TicTacToeLine
    {
        /// <summary>
        /// Gets the cells in this line.
        /// </summary>
        public List<TicTacToeCell> Cells { get; } = new List<TicTacToeCell>();

        /// <summary>
        /// Whether this line can be won by the given player with only one mark.
        /// </summary>
        /// <param name="player">The player to check for.</param>
        /// <returns>True if this line can be won, otherwise false.</returns>
        public bool CanBeWonByPlayer(string player)
        {
            return this.Cells.Where(c => c.Mark == player).Count() == this.Cells.Count - 1
                && this.Cells.Where(c => c.Mark == null).Count() == 1;
        }

        /// <summary>
        /// Whether it is possible for the given player to win this line (with any number of marks).
        /// </summary>
        /// <param name="player">The player to check for.</param>
        /// <returns>True if the player can win this line, otherwise false</returns>
        public bool IsWinnableByPlayer(string player)
        {
            return this.Cells.Where(c => (c.Mark ?? player) != player).Count() == 0;
        }

        /// <summary>
        /// Gets the first unmarked cell of this line.
        /// </summary>
        /// <returns>An unmarked cell.</returns>
        public TicTacToeCell GetFirstUnmarkedCell()
        {
            return this.Cells.First(x => x.Mark == null);
        }
    }
}
