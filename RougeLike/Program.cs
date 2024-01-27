using System;
using System.Collections.Generic;

class Program
{
    static char[,] maze = new char[20, 20];
    static int playerX;
    static int playerY;
    static int exitX = 18;
    static int exitY = 18;
    static List<Enemy> enemies = new List<Enemy>();
    static Random random = new Random();

    class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    static void Main()
    {
        GenerateMaze();
        GenerateEnemies();
        playerX = 0;
        playerY = 0;

        ConsoleKeyInfo keyInfo;

        do
        {
            Console.Clear();
            DrawMaze();
            DrawEnemies();
            MoveEnemies();
            keyInfo = Console.ReadKey();
            HandleKeyPress(keyInfo);
        } while (playerX != exitX || playerY != exitY);

        Console.Clear();
        Console.WriteLine("Поздравляю! Вы победили!");
    }

    static void GenerateMaze()
    {
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

        // Генерация лабиринта
        RecursiveBacktracker(1, 1);

        // Установка выхода с свободным пространством вокруг
        maze[exitX, exitY] = '.';
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
            } while (maze[enemyX, enemyY] != '.' || IsTooCloseToPlayerOrExit(enemyX, enemyY));

            enemies.Add(new Enemy(enemyX, enemyY));
        }
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
                if (x == playerX && y == playerY)
                {
                    Console.Write("P ");
                }
                else if (x == exitX && y == exitY)
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

    static void HandleKeyPress(ConsoleKeyInfo keyInfo)
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
    }
    static void MoveEnemies()
    {
        foreach (var enemy in enemies)
        {
            int direction = random.Next(4);
            int newEnemyX = enemy.X + (direction == 0 ? 1 : direction == 1 ? -1 : 0);
            int newEnemyY = enemy.Y + (direction == 2 ?
            1 : direction == 3 ? -1 : 0);

            if (newEnemyX >= 0 && newEnemyX < 20 && newEnemyY >= 0 && newEnemyY < 20 && maze[newEnemyX, newEnemyY] != '█' && !IsTooCloseToPlayerOrExit(newEnemyX, newEnemyY))
            {
                enemy.X = newEnemyX;
                enemy.Y = newEnemyY;
            }
        }
    }
}