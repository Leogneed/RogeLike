namespace RougeLike
{
    internal class Drawer
    {
        static char[,] maze = new char[20, 20];
        static List<Enemy> enemies = new List<Enemy>();
        static int playerX, playerY;
        static int playerHealth;
        static bool inBattle;
        static int exitX = 18;
        static int exitY = 18;

        public Drawer(char[,] _maze, int _playerX, int _playerY, List<Enemy> _enemies, int HP, bool battle)
        {
            maze = _maze;
            enemies = _enemies;
            playerX = _playerX;
            playerY = _playerY;
            playerHealth = HP;
            inBattle = battle;
        }

        public void DrawMaze()
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    if (x == exitX && y == exitY)
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write(maze[x, y] + " ");
                    }

                }
                Console.WriteLine();
            }
        }
        public void DrawEnemies()
        {
            foreach (var enemy in enemies)
            {
                Console.SetCursorPosition(enemy.X * 2, enemy.Y);
                Console.Write("E ");
            }
        }
        public void DrawPlayer()
        {
            Console.SetCursorPosition(playerX * 2, playerY);
            Console.Write("P ");
        }
        public void DrawPlayerHealth()
        {
            Console.SetCursorPosition(0, 21);
            Console.Write($"Здоровье игрока: {playerHealth}");
            Console.SetCursorPosition(0, 22);
            Console.Write($"Координаты игрока: ({playerX}, {playerY})");
        }
        public void DrawBattleStatus()
        {
            if (inBattle)
            {
                Console.SetCursorPosition(0, 23);
                Console.Write("Идет бой с врагом (");

                foreach (var enemy in enemies)
                {
                    if (Math.Abs(playerX - enemy.X) == 1 && Math.Abs(playerY - enemy.Y) == 0 || Math.Abs(playerX - enemy.X) == 0 && Math.Abs(playerY - enemy.Y) == 1)
                    {
                        Console.Write($"E({enemy.Health})");
                    }

                }
                Console.Write("). Нажмите F для атаки.");
            }
        }
    }
}
