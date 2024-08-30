using System;

class TicTacToe
{
    static char[] board = new char[9];
    static char currentPlayer = 'X';

    static void Main(string[] args)
    {
        do
        {
            InitializeBoard();
            PlayGame();
        } while (AskToPlayAgain());
    }

    static void InitializeBoard()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = (i + 1).ToString()[0];
        }
    }

    static void PlayGame()
    {
        while (true)
        {
            PrintBoard();
            Console.WriteLine($"Player {currentPlayer} turn:");
            int move = GetMove();

            if (IsValidMove(move))
            {
                MakeMove(move);
                if (CheckWin())
                {
                    PrintBoard();
                    Console.WriteLine($"Player {currentPlayer} wins!");
                    break;
                }
                if (IsBoardFull())
                {
                    PrintBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }
                SwitchPlayer();
            }
            else
            {
                Console.WriteLine("Invalid move! Try again.");
            }
        }
    }

    static bool AskToPlayAgain()
    {
        Console.WriteLine("Do you want to play again? (y/n)");
        string response = Console.ReadLine().ToLower();
        return response == "y" || response == "yes";
    }

    static void PrintBoard()
    {
        Console.Clear();
        Console.WriteLine(" {0} | {1} | {2} ", board[0], board[1], board[2]);
        Console.WriteLine("---|---|---");
        Console.WriteLine(" {0} | {1} | {2} ", board[3], board[4], board[5]);
        Console.WriteLine("---|---|---");
        Console.WriteLine(" {0} | {1} | {2} ", board[6], board[7], board[8]);
    }

    static int GetMove()
    {
        int move;
        while (!int.TryParse(Console.ReadLine(), out move) || move < 1 || move > 9)
        {
            Console.WriteLine("Invalid input! Enter a number between 1 and 9.");
        }
        return move - 1;
    }

    static bool IsValidMove(int move)
    {
        return board[move] != 'X' && board[move] != 'O';
    }

    static void MakeMove(int move)
    {
        board[move] = currentPlayer;
    }

    static void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
    }

    static bool CheckWin()
    {
        return (CheckRows() || CheckColumns() || CheckDiagonals());
    }

    static bool CheckRows()
    {
        return (board[0] == currentPlayer && board[1] == currentPlayer && board[2] == currentPlayer) ||
               (board[3] == currentPlayer && board[4] == currentPlayer && board[5] == currentPlayer) ||
               (board[6] == currentPlayer && board[7] == currentPlayer && board[8] == currentPlayer);
    }

    static bool CheckColumns()
    {
        return (board[0] == currentPlayer && board[3] == currentPlayer && board[6] == currentPlayer) ||
               (board[1] == currentPlayer && board[4] == currentPlayer && board[7] == currentPlayer) ||
               (board[2] == currentPlayer && board[5] == currentPlayer && board[8] == currentPlayer);
    }

    static bool CheckDiagonals()
    {
        return (board[0] == currentPlayer && board[4] == currentPlayer && board[8] == currentPlayer) ||
               (board[2] == currentPlayer && board[4] == currentPlayer && board[6] == currentPlayer);
    }

    static bool IsBoardFull()
    {
        foreach (char spot in board)
        {
            if (spot != 'X' && spot != 'O')
            {
                return false;
            }
        }
        return true;
    }
}