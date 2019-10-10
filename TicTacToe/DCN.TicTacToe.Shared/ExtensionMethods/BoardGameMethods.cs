using DCN.TicTacToe.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.ExtensionMethods
{
    public static class BoardGameMethods
    {
        public static int[,] SwapZvO(this int[,] array)
        {
            for (int j = 0; j < array.GetLength(0); j++)
            {
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    if (array[j, i].Equals(0))
                    {
                        array[j, i] = 1;
                    }
                    else if (array[j, i].Equals(1))
                    {
                        array[j, i] = 0;
                    }
                }
            }
            return array;
        }

        public static StatusGame GetStatementGame(this int[,] array)
        {
            if(TicTacToe_IsWin(array))
            {
                return StatusGame.Win;
            }
            else
            {
                if(array.TicTacToe_IsFull())
                {
                    return StatusGame.Tie;
                }
            }
            return StatusGame.Continue;
        }

        // Check three values to see if they are the same. If so, we have a winner.

        private static bool TicTacToe_row(int chOne, int chTwo, int chThree)
        {
            if ((chOne == chTwo) && (chOne == chThree) && 
                (chOne != (int)Game.SPACE && chTwo != (int)Game.SPACE && chThree != (int)Game.SPACE))
            {
                return true;
            }
            return false;
        }

        // Check board for a win by looping through rows, columns and checking diagonals.
        // If any of them are true, then there is a winning condition.
        public static bool TicTacToe_IsWin(this int [,] gameBoard)
        {
            // Loop through the rows
            for (int i = 0; i < 3; i++)
            {
                if (TicTacToe_row(gameBoard[i,0], gameBoard[i,1], gameBoard[i,2]))
                {
                    return true;
                }
            }

            // Loop through the columns
            for (int i = 0; i < 3; i++)
            {
                if (TicTacToe_row(gameBoard[0,i], gameBoard[1,i], gameBoard[2,i]))
                {
                    return true;
                }
            }

            // Check diagonals
            if (TicTacToe_row(gameBoard[0,0], gameBoard[1,1], gameBoard[2,2]))
            {
                return true;
            }

            if (TicTacToe_row(gameBoard[0,2], gameBoard[1,1], gameBoard[2,0]))
            {
                return true;
            }

            return false;
        }

        public static bool TicTacToe_IsFull(this int[,]gameBoard)
        {
            for(int j = 0; j < gameBoard.GetLength(0); j++)
            {
                for(int i = 0; i < gameBoard.GetLength(1); i++)
                {
                    if(gameBoard[j, i].Equals((int)Game.SPACE))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
    }
}
