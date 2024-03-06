namespace RougeLike
{
    class Program
    {
        static async Task Main()
        {
            StartGame startGame = new StartGame();
            startGame.Start();
            startGame.Update();
        }
    }
}
