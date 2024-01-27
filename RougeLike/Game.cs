namespace RougeLike
{
    public static class Game
    {
        public static void PlayGame()
        {
            int width = 20;
            int height = 10;
            Maze maze = MazeGenerator.GenerateMaze(width, height);
            Player player = new Player(0, 0);
            List<Enemy> enemies = EnemyGenerator.GenerateEnemies(maze, 5);

            while (true)
            {
                // Отрисовка лабиринта и игрока
                MazeRenderer.DrawMaze(maze);
                Console.WriteLine($"Player health: {player.Health}");
                Console.WriteLine($"Enemies left: {enemies.Count}");
                Console.WriteLine($"Player position: ({player.X}, {player.Y})");

                // Проверка условия победы
                if (player.X == width - 1 && player.Y == height - 1)
                {
                    Console.WriteLine("You win!");
                    break;
                }

                // Обработка ввода игрока
                Console.Write("Enter direction (w/a/s/d): ");
                string direction = Console.ReadLine();
                int dx = 0;
                int dy = 0;

                if (direction == "w")
                {
                    dy = -1;
                }
                else if (direction == "a")
                {
                    dx = -1;
                }
                else if (direction == "s")
                {
                    dy = 1;
                }
                else if (direction == "d")
                {
                    dx = 1;
                }

                int nx = player.X + dx;
                int ny = player.Y + dy;

                // Проверка на столкновение со стеной
                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                {
                    Console.WriteLine("You can't go there!");
                    continue;
                }

                // Проверка на столкновение с противником
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.X == nx && enemy.Y == ny)
                    {
                        player.Health -= enemy.Damage;
                        enemy.Health -= player.Damage;

                        if (enemy.Health <= 0)
                        {
                            enemies.Remove(enemy);
                        }

                        if (player.Health <= 0)
                        {
                            Console.WriteLine("You died!");
                            return;
                        }
                    }
                }

                // Обновление позиции игрока
                if ((maze.Map[player.X, player.Y] & 1 << 2 * dx + dy) == 0)
                {
                    player.X = nx;
                    player.Y = ny;
                }
            }
        }
    }
}