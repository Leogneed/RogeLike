namespace RougeLike
{
    public static class EnemyGenerator
    {
        private static Random random = new Random();

        public static List<Enemy> GenerateEnemies(Maze maze, int numEnemies)
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < numEnemies; i++)
            {
                while (true)
                {
                    int x = random.Next(0, maze.Width);
                    int y = random.Next(0, maze.Height);

                    if (maze.Map[x, y] == 0)
                    {
                        enemies.Add(new Enemy(x, y));
                        break;
                    }
                }
            }

            return enemies;
        }
    }
}