using System.Collections.Generic;
using HttpPuzzler.TicTacToe;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System;

namespace HttpPuzzler
{
    class Program
    {
        static void Main(string[] args)
        {
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
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TicTacToeRequest));
            //ser.WriteObject(stream1, request);
            /*
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.Write("JSON form of Person object: ");
            //Console.WriteLine(sr.ReadToEnd());
            StreamWriter writer = new StreamWriter("test.json");
            writer.Write(sr.ReadToEnd());
            writer.Close();*/

            FileStream stream = new FileStream("test.json", FileMode.Open);
            var request = (TicTacToeRequest)ser.ReadObject(stream);
            stream.Close();

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

        [DataContract(Name ="params")]
        private class RequestParams
        {
            [DataMember(Name = "gameid")]
            public int GameID { get; set; }

            [DataMember(Name ="mark")]
            public string Mark { get; set; }

            [DataMember(Name ="gamestate")]
            public string[] GameState { get; set; }
        }
    }
}
