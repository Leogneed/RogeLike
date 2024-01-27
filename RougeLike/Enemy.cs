namespace RougeLike
{
    public class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            Health = 50;
            Damage = 5;
        }
    }
}
