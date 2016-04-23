using System;
using System.IO;
using System.Collections.Generic;
using AustinHarris.JsonRpc;

namespace HttpPuzzler
{
    class Program
    {
        private static GlobalRpcService globalRpcService;
        private static TicTacToe.RpcService ticTacToeService;

        static int Main(string[] args)
        {
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
