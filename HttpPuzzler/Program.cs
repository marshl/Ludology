using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpPuzzler
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe.TicTacToeBoard board = new TicTacToe.TicTacToeBoard();
            board.Columns = new List<TicTacToe.TicTacToeBoard.TicTacToeColumn>();
            for (int x = 0; x < 3; ++x)
            {
                board.Columns.Add(new TicTacToe.TicTacToeBoard.TicTacToeColumn());
                board[x].Cells = new List<TicTacToe.TicTacToeBoard.TicTacToeColumn.TicTacToeCell>();
                for (int y = 0; y < 3; ++y)
                {
                    var c = new TicTacToe.TicTacToeBoard.TicTacToeColumn.TicTacToeCell();
                    c.XIndex = x;
                    c.YIndex = y;
                    board[x].Cells.Add(c);
                }
            }
            var v = board.GetAllLines();
        }
    }
}
