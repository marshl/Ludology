//-----------------------------------------------------------------------
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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using AustinHarris.JsonRpc;

    /// <summary>
    /// The entry class of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The port to listen on
        /// </summary>
        private const int ListenPort = 8080;

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
            // Create an instance of the service so it can be registered to handle requests.
            globalRpcService = new GlobalRpcService();

            // Find all registerable game classes
            var gameTypeList = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where typeof(GameRpcService).IsAssignableFrom(assemblyType)
                                where !assemblyType.IsAbstract
                                select assemblyType).ToArray();

            // And attempt to register them
            var gameList = new List<GameRpcService>();
            foreach (Type gameType in gameTypeList)
            {
                GameRpcService game = (GameRpcService)Activator.CreateInstance(gameType);
                if (Register(game))
                {
                    gameList.Add(game);
                }
            }

            var rpcResultHandler = new AsyncCallback(
                state =>
                {
                    var async = (JsonRpcStateAsync)state;
                    var result = async.Result;
                    var writer = (StreamWriter)async.AsyncState;

                    writer.Write(result);
                    writer.Flush();
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
        /// <returns>Whether the registration was successful or not.</returns>
        private static bool Register(GameRpcService game)
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

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RegistrationResponse));
                var request = (RegistrationResponse)ser.ReadObject(httpResponse.GetResponseStream());
                if (request == null)
                {
                    Console.WriteLine("No request could be found in the resposne stream.");
                    return false;
                }
                else if (request.Error != null)
                {
                    Console.WriteLine($"The server responded with an error message: {request.Error}");
                    return false;
                }
                else
                {
                    Console.WriteLine($"{game.GameName} was registered successfully: {request.Result.Message}");
                    return true;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"An exception occurred when registereing the game {game.GameName}: {ex}");
                return false;
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

        /// <summary>
        /// The data contract for the response of a game registration
        /// </summary>
        [DataContract]
        private class RegistrationResponse
        {
            /// <summary>
            /// Gets or sets the result of the response.
            /// </summary>
            [DataMember(Name = "result")]
            public ResponseResult Result { get; set; }

            /// <summary>
            /// Gets or sets the error message of the response, if an error occurred.
            /// </summary>
            [DataMember(Name = "error")]
            public string Error { get; set; }

            /// <summary>
            /// Gets or sets the game ID
            /// </summary>
            [DataMember(Name = "id")]
            public int ID { get; set; }
        }

        /// <summary>
        /// The data contract for the result of a RegistrationResponse
        /// </summary>
        [DataContract(Name = "result")]
        private class ResponseResult
        {
            /// <summary>
            /// Gets or sets the result of the registration ("OK" if successful)
            /// </summary>
            [DataMember(Name = "ping")]
            public string Ping { get; set; }

            /// <summary>
            /// Gets or sets a more detailed message if the Ping message is not enough.
            /// </summary>
            [DataMember(Name = "message")]
            public string Message { get; set; }
        }
    }
}
