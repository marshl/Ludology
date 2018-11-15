//-----------------------------------------------------------------------
// <copyright file="SnapRpcService.cs" company="marshl">
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
namespace Ludology.Snap
{
    using System.Collections.Generic;
    using AustinHarris.JsonRpc;

    /// <summary>
    /// The RPC service for the Snap game
    /// </summary>
   /* public class SnapRpcService/: GameRpcService
    {
        /// <summary>
        /// Gets the description of the game to give to the server.
        /// </summary>
        public override string GameDescription
        {
            get
            {
                return "Snap description";
            }
        }

        /// <summary>
        /// Gets the name of the game to register to the server with.
        /// </summary>
        public override string GameName
        {
            get
            {
                return "SNAP";
            }
        }

        /// <summary>
        /// The callback for when the server asks for a Snap move.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="cards">The list of cards already in play (first card on bottom).</param>
        /// <returns>The message to send to the server.</returns>
        [JsonRpcMethod("Snap.NextMove")]
        public Dictionary<string, bool> OnNextMove(int gameId, string[] cards)
        {
            bool move;
            if (cards.Length < 2)
            {
                move = false;
            }
            else
            {
                string topCard = cards[cards.Length - 1];
                string beneathCard = cards[cards.Length - 2];
                move = topCard.Substring(0, topCard.Length - 1) == beneathCard.Substring(0, beneathCard.Length - 1);
            }

            return new Dictionary<string, bool>() { { "Snap", false } };
        }
    }*/
}
