//-----------------------------------------------------------------------
// <copyright file="RpcService.cs" company="marshl">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AustinHarris.JsonRpc;

    /// <summary>
    /// The class to respond to Tic-Tac-Toe JSON requests
    /// </summary>
    public class RpcService : GameRpcService
    {
        /// <summary>
        /// Gets the name of the game to register with.
        /// </summary>
        public override string GameName
        {
            get
            {
                return "TICTACTOE";
            }
        }

        /// <summary>
        /// Gets the description of the game to register with.
        /// </summary>
        public override string GameDescription
        {
            get
            {
                return "Some description";
            }
        }

        /// <summary>
        /// The method for when a next move is required
        /// </summary>
        /// <param name="gameid">The ID of the game being played.</param>
        /// <param name="mark">The player mark (either 'O' or 'X').</param>
        /// <param name="gamestate">The current board state, as a list of cells (either 'X', 'O' or null).</param>
        /// <returns>The position to place the next</returns>
        [JsonRpcMethod("TicTacToe.NextMove")]
        private Dictionary<string, int> OnNextMove(int gameid, string mark, string[] gamestate)
        {
            if (mark != "X" && mark != "O")
            {
                throw new ArgumentException("Mark must be either 'O' or 'X'");
            }

            if (gamestate.Count(x => x == null) == 0)
            {
                throw new ArgumentException("The game state must contain at least one empty cell.");
            }

            TicTacToeBoard board = new TicTacToeBoard(gamestate);
            TicTacToeSolver solver = new TicTacToeSolver(board);
            int result = solver.Solve(mark);

            return new Dictionary<string, int>() { { "position", result } };
        }

        /// <summary>
        /// The method for when the game has been completed.
        /// </summary>
        /// <param name="gameid">The ID of the game.</param>
        /// <param name="winner">Whether a player won the game or not.</param>
        /// <param name="mark">The mark that won the game (if applicable)</param>
        /// <param name="gamestate">The final state of the game.</param>
        [JsonRpcMethod("TicTacToe.Complete")]
        private void OnCompletion(int gameid, bool winner, string mark, string[] gamestate)
        {
            // No action yet
        }

        /// <summary>
        /// The method for when this client has performed an illegal move.
        /// </summary>
        /// <param name="gameid">The ID of the game.</param>
        /// <param name="message">The error message.</param>
        /// <param name="errorcode">The current state of the game.</param>
        [JsonRpcMethod("TicTacToe.Error")]
        private void OnError(int gameid, string message, int errorcode)
        {
            // No action yet
        }
    }
}
