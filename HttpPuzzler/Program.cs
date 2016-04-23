using System.Runtime.Serialization;
using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
//using Jayrock.JsonRpc.Web;
//using Jayrock.JsonRpc;
using System.Diagnostics;
using AustinHarris.JsonRpc;
using AustinHarris.JsonRpc.Handlers.AspNet;

namespace HttpPuzzler
{
    /*class MyService : JsonRpcService
    {
        [JsonRpcMethod]
        public object Add(int a, int b)
        {
            return a + b;
        }

        [JsonRpcMethod]
        public object Echo(object obj)
        {
            return obj;
        }
    }*/

    class Program
    {

        private static object _svc;
        static int Main(string[] args)
        {
            /*
            try
            {
                var service = new MyService();
                var dispatcher = JsonRpcDispatcherFactory.CreateDispatcher(service);
                dispatcher.Process(Console.In, Console.Out);
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Trace.TraceError(e.ToString());
                return 1;
            }
            */

            /*var rpcResultHandler = new AsyncCallback(_ => Console.WriteLine(((JsonRpcStateAsync)_).Result));

            for (string line = Console.ReadLine(); !string.IsNullOrEmpty(line); line = Console.ReadLine())
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, null);
                async.JsonRpc = line;
                JsonRpcProcessor.Process(async);
            }
            */

            // must new up an instance of the service so it can be registered to handle requests.
            _svc = new exampleService();

            var rpcResultHandler = new AsyncCallback(
                state =>
                {
                    var async = ((JsonRpcStateAsync)state);
                    var result = async.Result;
                    var writer = ((StreamWriter)async.AsyncState);

                    writer.WriteLine(result);
                    writer.FlushAsync();
                });

            SocketsExample.SocketListener.start(3333, (writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer) { JsonRpc = line };
                JsonRpcProcessor.Process(async, writer);
            });
            return 0;
            /*Person p = new Person();
            p.name = "John";
            p.age = 42;*/
            /*TicTacToeRequest request = new TicTacToeRequest();
            request.Method = "TicTacToe.NextMove";
            request.Parameters = new RequestParams();
            request.Parameters.GameID = 78;
            request.Parameters.Mark = "X";
            request.Parameters.GameState = new string[] { "O", "X", "O", null, "O", null, null, "X", null };
            */

            /*
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TicTacToeRequest));
            */
            //ser.WriteObject(stream1, request);
            /*
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.Write("JSON form of Person object: ");
            //Console.WriteLine(sr.ReadToEnd());
            StreamWriter writer = new StreamWriter("test.json");
            writer.Write(sr.ReadToEnd());
            writer.Close();*/
            /*
            FileStream stream = new FileStream("test.json", FileMode.Open);
            var request = (TicTacToeRequest)ser.ReadObject(stream);
            stream.Close();
            */
            /*
            TicTacToeBoard board = new TicTacToeBoard();
            board.Columns = new List<TicTacToeColumn>();
            for (int x = 0; x < 3; ++x)
            {
                board.Columns.Add(new TicTacToeColumn());
                board[x].Cells = new List<TicTacToeCell>();
                for (int y = 0; y < 3; ++y)
                {
                    var c = new TicTacToeCell();
                    c.XIndex = x;
                    c.YIndex = y;
                    board[x].Cells.Add(c);
                }
            }
            var v = board.GetAllLines();*/
        }
        public class HelloWorld : JsonRpcHandler
        {
            [JsonRpcMethod("greetings")]
            public string Greetings()
            {
                return "Welcome to Jayrock!";
            }
        }


        class exampleService : JsonRpcService
        {
            [JsonRpcMethod] // handles JsonRpc like : {'method':'incr','params':[5],'id':1}
            private int incr(int i) { return i + 1; }

            [JsonRpcMethod] // handles JsonRpc like : {'method':'decr','params':[5],'id':1}
            private int decr(int i) { return i - 1; }

            [JsonRpcMethod]
            private object Ping()
            {
                //JsonResponse response = new JsonResponse();
                var r = new Dictionary<string, string>();
                r.Add("result", "OK");
                //response.Result = r;
                return r;// esponse;
            }

        }

        [DataContract]
        internal class Person
        {
            [DataMember]
            internal string name;

            [DataMember]
            internal int age;
        }

        [DataContract]
        private class TicTacToeRequest
        {
            [DataMember(Name = "method")]
            public string Method { get; set; }

            [DataMember(Name = "params")]
            public RequestParams Parameters { get; set; }

            [DataMember(Name = "id")]
            public int ID { get; set; }
        }

        [DataContract(Name = "params")]
        private class RequestParams
        {
            [DataMember(Name = "gameid")]
            public int GameID { get; set; }

            [DataMember(Name = "mark")]
            public string Mark { get; set; }

            [DataMember(Name = "gamestate")]
            public string[] GameState { get; set; }
        }
        /*
        public class ExampleCalculatorService : JsonRpcService
        {
            [JsonRpcMethod]
            private double add(double l, double r)
            {
                return l + r;
            }
        }

        class ConsoleServer
        {
            static object[] services = new object[] {
           new ExampleCalculatorService()
        };
        }*/
    }
}

namespace SocketsExample
{
    public class SocketListener
    {
        public static void start(int listenPort, Action<StreamWriter, string> handleRequest)
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), listenPort);
            server.Start();
            Console.WriteLine(" You can connected with Putty on a (RAW session) to {0} to issue JsonRpc requests.", server.LocalEndpoint);
            while (true)
            {
                try
                {
                    using (var client = server.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    {
                        Console.WriteLine("Client Connected..");
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var writer = new StreamWriter(stream, new UTF8Encoding(false));

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            handleRequest(writer, line);
                            Console.WriteLine("REQUEST: {0}", line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("RPCServer exception " + e);
                }
            }
        }
    }
}