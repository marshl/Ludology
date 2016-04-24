//-----------------------------------------------------------------------
// <copyright file="TicTacToeCell.cs" company="marshl">
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
namespace HttpPuzzler.TicTacToe
{
    /// <summary>
    /// A single cell in a TicTacToeBoard
    /// </summary>
    public class TicTacToeCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeCell"/> class.
        /// </summary>
        /// <param name="x">The x index of the cell.</param>
        /// <param name="y">The y index of the cell.</param>
        /// <param name="mark">The mark of the cell.</param>
        public TicTacToeCell(int x, int y, string mark)
        {
            this.Mark = mark;
            this.XIndex = x;
            this.YIndex = y;
        }

        /// <summary>
        /// Gets the mark of the cell.
        /// </summary>
        public string Mark { get; }

        /// <summary>
        /// Gets the x index of the cell.
        /// </summary>
        public int XIndex { get; }

        /// <summary>
        /// Gets the y index of the cell.
        /// </summary>
        public int YIndex { get; }

        /// <summary>
        /// Gets or sets the number of lines that this cells lies within that are winnable by the current player.
        /// </summary>
        public int WinningLineCount { get; set; }

        /// <summary>
        /// Gets or sets the number of cells within lines this cell passes through that are marked by the current player.
        /// </summary>
        public int AdjacentCellCount { get; set; }
    }
}
