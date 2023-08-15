namespace Battleship_v2.Utility
{
    public sealed class Move
    {
        public int X { get; set; }
        public int Y { get; set; }
        public (int, int) Position
        {
            get => (X, Y);
            set
            {
                X = value.Item1;
                Y = value.Item2;
            }
        }
        public bool WasHit { get; set; } = false;

        public Move( int theXPos, int theYPos )
        {
            X = theXPos;
            Y = theYPos;
        }

        public Move( (int, int) thePosition )
        {
            Position = thePosition;
        }
    }
}
