namespace RougeLike
{
    class GenerateMaze
    {
        static char[,] maze = new char[20, 20];
        static int exitX = 18;
        static int exitY = 18;
        public void Generate()
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



        public char[,] getMaze()
        {
            return maze;
        }
    }
}
