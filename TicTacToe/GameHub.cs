using Microsoft.AspNetCore.SignalR;

namespace TicTacToe;

public class GameHub : Hub
{
    private static string[,] board = new string[3, 3];

    public async Task SendMove(string player, int row, int col)
    {
        board[row, col] = player;
        await Clients.All.SendAsync("ReceiveMove", player, row, col);

        if (CheckWinner(player))
        {
            await Clients.All.SendAsync("ReceiveWinner", player);
            ResetBoard();
            //await Clients.All.SendAsync("ReceiveReset");
        }
    }

    public async Task SendReset()
    {
        ResetBoard();
        await Clients.All.SendAsync("ReceiveReset");
    }

    private bool CheckWinner(string player)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                return true;
        }
        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
                return true;
        }
        // Check diagonals
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            return true;

        return false;
    }

    private void ResetBoard()
    {
        board = new string[3, 3];
    }
}
