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
namespace Ludophile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        /// The RPC service for TicTacToe specific methods.
        /// </summary>
        private static TicTacToe.RpcService ticTacToeService;

        /// <summary>
        /// The entry point of the program
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The error code.</returns>
        public static int Main(string[] args)
        {
            /*JsonRpcClient
            Uri remoteUri = ;
            //var client = AustinHarris.JsonRpc.
            var rpcResultHandler = new AsyncCallback(_ => Console.WriteLine(((JsonRpcStateAsync)_).Result));
            var async = new JsonRpcStateAsync(rpcResultHandler, null);
            async.JsonRpc = "{'method':'add','params':[1,2],'id':1}";
            JsonRpcProcessor.Process(async);*/
            /*{
                AutoResetEvent are = new AutoResetEvent(false);
                var rpc = new JsonRpcClient(new Uri("http://localhost:49718/json.rpc"));
                string method = "RegistrationService.Register";
                JsonResponse<Newtonsoft.Json.Linq.JObject> responseResult = null;
                var myObs = rpc.Invoke<Newtonsoft.Json.Linq.JObject>(method, null, Scheduler.TaskPool);

                myObs.Subscribe(
                    onNext: x =>
                    {
                        responseResult = x;
                        are.Set();
                    },
                    onError: x =>
                    {
                        are.Set();
                    },
                    onCompleted: () => { are.Set(); }
                    );

                are.WaitOne();
                var res = responseResult?.Result;
            }*/

            /*{
                WebClient client = new WebClient();
                string data = "{ 'method':'Status.Ping', 'id':1}";
                var bindata = new byte[data.Length * sizeof(char)];
                System.Buffer.BlockCopy(data.ToCharArray(), 0, bindata, 0, data.Length);
                client.UploadData("http://127.0.0.1:3333", bindata);

            }*/

            /*{
                string method = "Status.Ping";
                string input = "Echo this sucka";
                string id = "1";
                string callbackName = "myCallback";
                object[] parameters = new object[1];
                parameters[0] = input;

                JsonRequest jsonParameters = new JsonRequest()
                {
                    Method = method,
                    Params = parameters,
                    Id = id
                };

                var serailaizedParameters = Newtonsoft.Json.JsonConvert.SerializeObject(jsonParameters);
                var postData = string.Format("jsonrpc={0}&callback={1}", serailaizedParameters, callbackName);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://127.0.0.1:3333"));
                request.ContentType = "application/json-rpc";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.KeepAlive = false;
                /*byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postData);
                request.ContentLength = bytes.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }* /
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                try
                {
                    requestWriter.Write("{'method':'Status.Ping','id':1}");
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //myCallback({"jsonrpc":"2.0","result":"Echo this sucka","id":"1"})
                var regexPattern = callbackName + @"\({.*}\)";
                var result = reader.ReadToEnd().Trim();
                
                //Assert.IsTrue(Regex.IsMatch(result, regexPattern));
            }*/

            /*{
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3333");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{ \"method\": \"Status.Ping\", \"id\":1 }";

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    //Now you have your response.
                    //or false depending on information in the response
                }
            }*/

            /*AutoResetEvent are = new AutoResetEvent(false);
            var client = new JsonRpcClient(new Uri("http://localhost:49718/json.rpc"));
            var myObs = client.Invoke<string>("RegistrationService.Register", "My Message", TaskPoolScheduler.Default);
            WebClient wc = new WebClient();
            wc.
            using (myObs.Subscribe(
                onNext: x =>
                {
                    Console.WriteLine(x.Result);
                },
                onError: x =>
                {
                    Debug.Assert(false);
                },
                onCompleted: () => are.Set()
                ))
            {
                are.WaitOne();
           }*/

            /*var myAsync = new JsonRpcStateAsync();
            myAsync.JsonRpc = "{method:'yourMethod',params:[],id:0}";
            var myCusomContext = new { }; // ..Anything..; But be consistant. :)
            JsonRpcProcessor.Process(myAsync, myCustomContext);*/

            /*var solver = new TicTacToe.TicTacToeSolver(new TicTacToe.TicTacToeBoard(new string[] {
                "O", "X", "O",
                null, null, null,
                null, null, "X" }));
            var place = solver.Solve("X");*/

            // must new up an instance of the service so it can be registered to handle requests.
            globalRpcService = new GlobalRpcService();
            ticTacToeService = new TicTacToe.RpcService();

            var rpcResultHandler = new AsyncCallback(
                state =>
                {
                    var async = (JsonRpcStateAsync)state;
                    var result = async.Result;
                    var writer = (StreamWriter)async.AsyncState;

                    writer.WriteLine(result);
                    writer.FlushAsync();
                });

            SocketListener.start(
                3333,
                (writer, line) => 
                {
                    var async = new JsonRpcStateAsync(rpcResultHandler, writer) { JsonRpc = line };
                    JsonRpcProcessor.Process(async, writer);
                });

            return 0;
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
