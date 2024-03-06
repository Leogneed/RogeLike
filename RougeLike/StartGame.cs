namespace RougeLike
{
    class StartGame
    {
        static char[,] Maze = new char[20, 20];
        static int playerX = 0;
        static int playerY = 0;
        static int exitX = 18;
        static int exitY = 18;
        static int playerHealth = 10;
        static bool inBattle = false;
        static List<Enemy> enemies = new List<Enemy>();

        public void Start()
        {
            GenerateMaze maze = new GenerateMaze();
            maze.Generate();
            Maze = maze.getMaze();

            GenerateEnemies enemy = new GenerateEnemies(Maze, playerX, playerY, enemies);
            enemy.Generate();

            
        }

        public async void Update()
        {
                
            while (true)
            {
                Control control = new Control(Maze, playerX, playerY, enemies, inBattle);
                Drawer drawer = new Drawer(Maze, playerX, playerY, enemies, playerHealth, inBattle);
                Console.Clear();
                drawer.DrawMaze();
                drawer.DrawPlayer();
                drawer.DrawPlayerHealth();
                control.MoveEnemies();
                drawer.DrawEnemies();
                CheckCollisions();
                drawer.DrawBattleStatus();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                await control.HandleKeyPressAsync(keyInfo, drawer);
                Maze = control.GetMaze();
                playerX = control.GetPlayerX();
                playerY = control.GetPlayerY();
                inBattle = control.IsInBattle();
                enemies = control.GetEnemies();
            }
        }
        static void CheckCollisions()
        {
            // Проверка на выигрыш
            if (playerX == exitX && playerY == exitY)
            {
                Console.Clear();
                Console.WriteLine("Поздравляю! Вы победили!");
                Environment.Exit(0);
            }
        }
    }
}
