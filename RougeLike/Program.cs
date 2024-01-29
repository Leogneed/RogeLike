using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rogalik
{
    class Program
    {
        static char[,] maze = new char[20, 20];
        static int playerX = 0;
        static int playerY = 0;
        static int exitX = 18;
        static int exitY = 18;
        static int playerHealth = 10;
        static bool inBattle = false;
        static List<Enemy> enemies = new List<Enemy>();
        static Random random = new Random();
        class Enemy
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Health { get; set; }

            public Enemy(int x, int y)
            {
                X = x;
                Y = y;
                Health = 3;
            }
        }
        static async Task Main()
        {
            GenerateMaze();
            GenerateEnemies();

            while (true)
            {
                Console.Clear();
                DrawMaze();
                DrawPlayer();
                DrawPlayerHealth();
                MoveEnemies();
                DrawEnemies();
                CheckCollisions();
                DrawBattleStatus();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                await HandleKeyPressAsync(keyInfo);

            }
        }

        static void GenerateMaze()
        {
            //Random random = new Random();

            // Заполнение лабиринта символами стен
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    maze[x, y] = '█';
                }
            }

            // Размещение входа рядом с игроком
            maze[0, 1] = '.';
            maze[1, 0] = '.';

            // Генерация лабиринта с гарантированным путем от входа к выходу
            RecursiveBacktracker(1, 1);

            // Установка выхода с свободным пространством вокруг
            maze[exitX, exitY] = 'X';
            maze[Math.Max(exitX - 1, 0), exitY] = '.';
            maze[Math.Min(exitX + 1, 19), exitY] = '.';
            maze[exitX, Math.Max(exitY - 1, 0)] = '.';
            maze[exitX, Math.Min(exitY + 1, 19)] = '.';

            // Очистка позиции игрока
            maze[0, 0] = '.';
        }

        static void GenerateEnemies()
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

        static bool IsEnemyOnCell(int x, int y)
        {
            return enemies.Any(enemy => enemy.X == x && enemy.Y == y);
        }

        static bool IsTooCloseToPlayerOrExit(int x, int y)
        {
            return Math.Abs(x - playerX) < 5 || Math.Abs(y - playerY) < 5 || Math.Abs(x - exitX) < 5 || Math.Abs(y - exitY) < 5;
        }

        static void RecursiveBacktracker(int x, int y)
        {
            maze[x, y] = '.';

            int[] directions = { 0, 1, 2, 3 };
            ShuffleArray(directions);

            foreach (int direction in directions)
            {
                int nextX = x + 2 * (direction == 0 ? 1 : direction == 1 ? -1 : 0);
                int nextY = y + 2 * (direction == 2 ? 1 : direction == 3 ? -1 : 0);

                if (nextX > 0 && nextX < 19 && nextY > 0 && nextY < 19 && maze[nextX, nextY] == '█')
                {
                    maze[x + (direction == 0 ? 1 : direction == 1 ? -1 : 0), y + (direction == 2 ? 1 : direction == 3 ? -1 : 0)] = '.';
                    RecursiveBacktracker(nextX, nextY);
                }
            }
        }

        static void ShuffleArray(int[] array)
        {
            Random random = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        static void DrawMaze()
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

        static void DrawEnemies()
        {
            foreach (var enemy in enemies)
            {
                Console.SetCursorPosition(enemy.X * 2, enemy.Y);
                Console.Write("E ");
            }
        }
        static void DrawPlayer()
        {
            Console.SetCursorPosition(playerX * 2, playerY);
            Console.Write("P ");
        }
        static void DrawPlayerHealth()
        {
            Console.SetCursorPosition(0, 21);
            Console.Write($"Здоровье игрока: {playerHealth}");
            Console.SetCursorPosition(0, 22);
            Console.Write($"Координаты игрока: ({playerX}, {playerY})");
        }
        static void DrawBattleStatus()
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
        static async Task HandleKeyPressAsync(ConsoleKeyInfo keyInfo)
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
                        DrawBattleStatus();

                        break;
                    }
                }
            }
        }
        static void MoveEnemies()
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
