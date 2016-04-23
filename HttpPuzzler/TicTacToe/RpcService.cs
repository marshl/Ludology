using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AustinHarris.JsonRpc;

namespace HttpPuzzler.TicTacToe
{
    class RpcService : JsonRpcService
    {
        [JsonRpcMethod("TicTacToe.NextMove")]
        private object TicTacToeNextMove(int gameid, string mark, string[] gamestate)
        {
            TicTacToeBoard board = new TicTacToeBoard(gamestate);
            TicTacToeSolver solver = new TicTacToeSolver(board);
            solver.Solve(mark);

            return new Dictionary<string, int>() { { "position", 1} };
        }
    }
}
