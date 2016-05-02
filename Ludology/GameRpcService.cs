//-----------------------------------------------------------------------
// <copyright file="GameRpcService.cs" company="marshl">
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
namespace Ludology
{
    using AustinHarris.JsonRpc;

    /// <summary>
    /// Every game must inherit from this class.
    /// Every object that inherits from GameRpcService will be found and registered at runtime.
    /// </summary>
    public abstract class GameRpcService : JsonRpcService
    {
        /// <summary>
        /// Gets the name of the game to send during registration.
        /// </summary>
        public abstract string GameName { get; }

        /// <summary>
        /// Gets the game description to send during registration.
        /// </summary>
        public abstract string GameDescription { get; }
    }
}
