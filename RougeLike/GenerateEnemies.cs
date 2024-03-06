namespace RougeLike
{
    internal class GenerateEnemies
    {
        static Random random = new Random();
        static char[,] maze = new char[20, 20];
        static int playerX, playerY;
        static int exitX = 18;
        static int exitY = 18;
        static List<Enemy> enemies = new List<Enemy>();

        public GenerateEnemies(char[,] _maze, int _playerX, int _playerY, List<Enemy> _enemies)
        {
            maze = _maze;
            playerX = _playerX;
            playerY = _playerY;
            enemies = _enemies;
        }

        public void Generate()
        {
            int numberOfEnemies = random.Next(3, 7);

            for (int i = 0; i < numberOfEnemies; i++)
            {
                int enemyX;
                int enemyY;

                do
                {
                    enemyX = random.Next(2, 18);
                    enemyY = random.Next(2, 18);
                } while (maze[enemyX, enemyY] != '.' || IsTooCloseToPlayerOrExit(enemyX, enemyY) || IsEnemyOnCell(enemyX, enemyY));

                enemies.Add(new Enemy(enemyX, enemyY));
            }
        }
        static bool IsTooCloseToPlayerOrExit(int x, int y)
        {
            return Math.Abs(x - playerX) < 5 || Math.Abs(y - playerY) < 5 || Math.Abs(x - exitX) < 5 || Math.Abs(y - exitY) < 5;
        }
        static bool IsEnemyOnCell(int x, int y)
        {
            return enemies.Any(enemy => enemy.X == x && enemy.Y == y);
        }
    }
}
