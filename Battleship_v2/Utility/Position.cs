using System.Windows.Controls;
using Battleship_v2.Models;

namespace Battleship_v2.Utility
{
    public sealed class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public (int, int) Coordinates
        {
            get => (X, Y);
            set
            {
                X = value.Item1;
                Y = value.Item2;
            }
        }
        public bool WasHit { get; set; } = false;

        public static bool operator ==(Position theLeft, Position theRight)
        {
            if (theLeft is null || theRight is null) return false;
            return (theLeft.X == theRight.X && theLeft.Y == theRight.Y);
        }

        public static bool operator !=( Position theLeft, Position theRight ) => !( theLeft == theRight );

        public override bool Equals(object theObject)
        {
            if (theObject == null || !(theObject is Position)) return false;
            Position aPosition = (Position)theObject;
            return (this.X == aPosition.X && this.Y == aPosition.Y);
        }

        public override int GetHashCode()
        {
            return ((X.GetHashCode() * 17) ^ (Y.GetHashCode() * 47) ^ (WasHit.GetHashCode() * 89));
        }

        public Position( int theXPos, int theYPos )
        {
            X = theXPos;
            Y = theYPos;
        }

        public Position() : this( -1, -1 ) { }

        public void SetValuesFrom(Position thePosition)
        {
            X = thePosition.X;
            Y = thePosition.Y;
            WasHit = thePosition.WasHit;
        }

        public bool IsValid()
        {
            return ( X >= 0 && X < ShipGridModel.GRID_SIZE && Y >= 0 && Y < ShipGridModel.GRID_SIZE );
        }

        public void Swap()
        {
            (X, Y) = (Y, X);
        }

        public override string ToString()
        {
            char aLetter = (char)(X + 65);
            return $"Position ({aLetter}/{Y+1})";
        }

        public Position Clone() => new Position( X, Y );

        public Position GetNeighbour( Orientation theOrientation = Orientation.Horizontal, int theDirection = 1 )
        {
            return theOrientation == Orientation.Horizontal ? new Position( X + theDirection, Y ) : new Position( X, Y + theDirection );
        }

        public void MoveRight() => X++;
        public void MoveLeft() => X--;
        public void MoveDown() => Y++;
        public void MoveUp() => Y--;
    }
}
