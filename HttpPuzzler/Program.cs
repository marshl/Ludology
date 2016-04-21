using System.Collections.Generic;
using HttpPuzzler.TicTacToe;

namespace HttpPuzzler
{
    class Program
    {
        static void Main(string[] args)
        {
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
            var v = board.GetAllLines();
        }
    }
}
