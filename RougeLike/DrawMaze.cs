using RougeLike;

namespace RougeLike
{
    public static class MazeRenderer
    {
        public static void DrawMaze(Maze maze)
        {
            char wall = '█';
            char path = ' ';

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    if ((maze.Map[x, y] & 1) == 0)
                    {
                        Console.Write(wall);
                    }
                    else
                    {
                        Console.Write(path);
                    }
                }
                Console.WriteLine();

                for (int x = 0; x < maze.Width; x++)
                {
                    if ((maze.Map[x, y] & 2) == 0)
                    {
                        Console.Write(wall);
                    }
                    else
                    {
                        Console.Write(path);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}