﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="marshl">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using AustinHarris.JsonRpc;

    /// <summary>
    /// The entry class of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The RRC service for global methods.
        /// </summary>
        private static GlobalRpcService globalRpcService;

        /// <summary>
        /// The entry point of the program
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The error code.</returns>
        public static int Main(string[] args)
        {
            // must new up an instance of the service so it can be registered to handle requests.
            globalRpcService = new GlobalRpcService();

            const int ListenPort = 8080;

            var listOfGameTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                   from assemblyType in domainAssembly.GetTypes()
                                   where typeof(GameRpcService).IsAssignableFrom(assemblyType)
                                   where !assemblyType.IsAbstract
                                   select assemblyType).ToArray();

            var listOfGames = new List<GameRpcService>();
            foreach (Type gameType in listOfGameTypes)
            {
                GameRpcService game = (GameRpcService)Activator.CreateInstance(gameType);
                Register(game);
                listOfGames.Add(game);
            }

            var rpcResultHandler = new AsyncCallback(
                state =>
                {
                    var async = (JsonRpcStateAsync)state;
                    var result = async.Result;
                    var writer = (StreamWriter)async.AsyncState;

                    writer.WriteAsync(result);
                    writer.FlushAsync();
                });

            SocketListener.start(
                ListenPort,
                (writer, line) =>
                {
                    var async = new JsonRpcStateAsync(rpcResultHandler, writer) { JsonRpc = line };
                    JsonRpcProcessor.Process(async, writer);
                });

            return 0;
        }

        /// <summary>
        /// Registers a game with the server.
        /// </summary>
        /// <param name="game">The game to register.</param>
        private static void Register(GameRpcService game)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://8557af3f.ngrok.io/rpc");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ServicePoint.Expect100Continue = false;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = File.ReadAllText("registration.json");
                json = json.Replace("<GAME_NAME>", game.GameName);
                json = json.Replace("<GAME_DESCRIPTION", game.GameDescription);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// The global RPC service
        /// </summary>
        private class GlobalRpcService : JsonRpcService
        {
            /// <summary>
            /// Returns an ok status to the server.
            /// </summary>
            /// <returns>The result.</returns>
            [JsonRpcMethod("Status.Ping")]
            public object Ping()
            {
                var r = new Dictionary<string, string>();
                r.Add("result", "OK");
                return r;
            }
        }
    }
}
