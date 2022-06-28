namespace Tictactoe;

internal class Program
{
    public static void Main()
    {
        while (true)
        {
            Game myGame = new Game();
            myGame.Start();
            Thread.Sleep(2000);
        }
        
    }
}
