using System;
using System.IO;
using System.Collections.Generic;
using AustinHarris.JsonRpc;
using System.Threading;
using System.Reactive.Concurrency;
using System.Diagnostics;

namespace HttpPuzzler
{
    class Program
    {
        private static GlobalRpcService globalRpcService;
        private static TicTacToe.RpcService ticTacToeService;

        static int Main(string[] args)
        {
            /*JsonRpcClient
            Uri remoteUri = ;
            //var client = AustinHarris.JsonRpc.
            var rpcResultHandler = new AsyncCallback(_ => Console.WriteLine(((JsonRpcStateAsync)_).Result));
            var async = new JsonRpcStateAsync(rpcResultHandler, null);
            async.JsonRpc = "{'method':'add','params':[1,2],'id':1}";
            JsonRpcProcessor.Process(async);*/
            {
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
            }
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
                    var async = ((JsonRpcStateAsync)state);
                    var result = async.Result;
                    var writer = ((StreamWriter)async.AsyncState);

                    writer.WriteLine(result);
                    writer.FlushAsync();
                });

            SocketListener.start(3333, (writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer) { JsonRpc = line };
                JsonRpcProcessor.Process(async, writer);
            });

            return 0;
        }

        private class GlobalRpcService : JsonRpcService
        {
            [JsonRpcMethod("Status.Ping")]
            private object Ping()
            {
                var r = new Dictionary<string, string>();
                r.Add("result", "OK");
                return r;
            }
        }
    }
}
