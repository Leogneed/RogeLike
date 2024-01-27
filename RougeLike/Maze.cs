namespace RougeLike
{
    public class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int[,] Map { get; set; }

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new int[width, height];
        }
    }
}
