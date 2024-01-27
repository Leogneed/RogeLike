namespace RougeLike
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Health = 100;
            Damage = 10;
        }
    }
}