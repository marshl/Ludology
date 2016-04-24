using System.Collections.Generic;
using AustinHarris.JsonRpc;

namespace HttpPuzzler.TicTacToe
{
    class RpcService : JsonRpcService
    {
        [JsonRpcMethod("TicTacToe.NextMove")]
        private Dictionary<string, int> TicTacToeNextMove(int gameid, string mark, string[] gamestate)
        {
            TicTacToeBoard board = new TicTacToeBoard(gamestate);
            TicTacToeSolver solver = new TicTacToeSolver(board);
            int result = solver.Solve(mark);

            return new Dictionary<string, int>() { { "position", result } };
        }


        [JsonRpcMethod("TicTacToe.Complete")]
        private void OnCompletion(int gameid, bool winner, string mark, string[] gamestate)
        {

        }

        [JsonRpcMethod("TicTacToe.Error")]
        private void OnError(int gameid, string message, int errorcode)
        {

        }
    }
}
