using RougeLike;

namespace RougeLike
{
    public static class MazeGenerator
    {
        private static Random random = new Random();

        public static Maze GenerateMaze(int width, int height)
        {
            Maze maze = new Maze(width, height);

            RecursiveBacktracker(0, 0, maze);

            return maze;
        }

        private static void RecursiveBacktracker(int x, int y, Maze maze)
        {
            List<(int, int)> directions = new List<(int, int)>()
            {
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1)
            };
            Shuffle(directions);

            foreach ((int dx, int dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= maze.Width || ny >= maze.Height)
                {
                    continue;
                }

                if (maze.Map[nx, ny] == 1)
                {
                    continue;
                }

                maze.Map[x, y] |= (1 << directions.IndexOf((dx, dy)));
                maze.Map[nx, ny] |= (1 << directions.IndexOf((-dx, -dy)));

                //RecursiveBacktracker(nx, ny, maze);
            }
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = random.Next(i, list.Count);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
