namespace RougeLike
{
    class Control
    {
        static char[,] maze = new char[20, 20];
        static List<Enemy> enemies = new List<Enemy>();
        static int playerX, playerY;
        static Random random = new Random();
        static bool inBattle;
        static int exitX = 18;
        static int exitY = 18;
        public Control(char[,] _maze, int _playerX, int _playerY, List<Enemy> _enemies, bool battle)
        {
            maze = _maze;
            enemies = _enemies;
            playerX = _playerX;
            playerY = _playerY;
            inBattle = battle;
        }

        public char[,] GetMaze() { return maze; }
        public int GetPlayerX() {  return playerX; }
        public int GetPlayerY() { return playerY; }
        public bool IsInBattle() { return inBattle; }
        public List<Enemy> GetEnemies() {  return enemies; }
        static bool IsTooCloseToPlayerOrExit(int x, int y)
        {
            return Math.Abs(x - playerX) < 5 || Math.Abs(y - playerY) < 5 || Math.Abs(x - exitX) < 5 || Math.Abs(y - exitY) < 5;
        }

        public async Task HandleKeyPressAsync(ConsoleKeyInfo keyInfo, Drawer drawer)
        {
            if (inBattle)
            {
                if (keyInfo.Key == ConsoleKey.F)
                {
                    AttackEnemy();
                }
            }
            else
            {
                int newPlayerX = playerX;
                int newPlayerY = playerY;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        newPlayerY--;
                        break;

                    case ConsoleKey.DownArrow:
                        newPlayerY++;
                        break;

                    case ConsoleKey.LeftArrow:
                        newPlayerX--;
                        break;

                    case ConsoleKey.RightArrow:
                        newPlayerX++;
                        break;
                }

                // Проверка на наличие стены перед движением игрока
                if (newPlayerX >= 0 && newPlayerX < 20 && newPlayerY >= 0 && newPlayerY < 20 && maze[newPlayerX, newPlayerY] != '█')
                {
                    playerX = newPlayerX;
                    playerY = newPlayerY;
                }

                // Проверка на начало боя, если игрок подошел к врагу
                foreach (var enemy in enemies)
                {
                    if (Math.Abs(playerX - enemy.X) == 1 && Math.Abs(playerY - enemy.Y) == 0 || Math.Abs(playerX - enemy.X) == 0 && Math.Abs(playerY - enemy.Y) == 1)
                    {
                        inBattle = true;
                        drawer.DrawBattleStatus();

                        break;
                    }
                }
            }
        }
        public void MoveEnemies()
        {
            foreach (var enemy in enemies)
            {
                int direction = random.Next(4);
                int newEnemyX = enemy.X + (direction == 0 ? 1 : direction == 1 ? -1 : 0);
                int newEnemyY = enemy.Y + (direction == 2 ? 1 : direction == 3 ? -1 : 0);

                if (newEnemyX >= 0 && newEnemyX < 20 && newEnemyY >= 0 && newEnemyY < 20 && maze[newEnemyX, newEnemyY] != '█' && !IsTooCloseToPlayerOrExit(newEnemyX, newEnemyY))
                {
                    enemy.X = newEnemyX;
                    enemy.Y = newEnemyY;
                }
            }
        }

        static void AttackEnemy()
        {
            foreach (var enemy in enemies)
            {
                if (Math.Abs(playerX - enemy.X) == 1 && Math.Abs(playerY - enemy.Y) == 0 || Math.Abs(playerX - enemy.X) == 0 && Math.Abs(playerY - enemy.Y) == 1)
                {
                    enemy.Health--;

                    Console.SetCursorPosition(26, 23); // Устанавливаем курсор в правильное место для обновления информации о здоровье врага
                    Console.Write($"{enemy.Health} "); // Обновляем информацию о здоровье врага

                    if (enemy.Health <= 0)
                    {
                        // Победа в бою
                        inBattle = false;
                        maze[enemy.X, enemy.Y] = '.'; // Заменяем врага на пустую клетку
                        enemies.Remove(enemy);
                        Console.Clear();
                        break;
                    }
                }
            }
        }
    }
}
