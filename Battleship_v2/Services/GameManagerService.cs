using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Battleship_v2.Models;
using Battleship_v2.Ships;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Services
{
    public class GameManagerService
    {
        public const int GRID_SIZE = 10;

        private ShipGridModel m_ShipGrid;
        private TargetInputModel m_TargetInput;
        private List<Ship> m_Ships;

        public GameManagerService InjectShipGridModel( ShipGridModel theModel )
        {
            m_ShipGrid = theModel;
            return this;
        }

        public GameManagerService InjectTargetInputModel( TargetInputModel theModel )
        {
            m_TargetInput = theModel;
            return this;
        }

        public static GameManagerService Instance { get; private set; } = new GameManagerService();
        private GameManagerService() { }

        private void init()
        {
            if (m_Ships != null)
            {
                return;
            }

            m_Ships = new List<Ship>()
            {
                new Carrier(),
                new Battleship(),
                new Battleship(),
                new Submarine(),
                new Submarine(),
                new Destroyer(),
                new Destroyer(),
                new PatrolBoat(),
                new PatrolBoat(),
                new PatrolBoat()
            };

            placeShipsRandomly();
        }

        private bool isColliding( Ship theShip )
        {
            foreach ( Ship aShip in m_Ships )
            {
                // Skip the iteration, when comparing against itself.
                if ( aShip == theShip )
                {
                    continue;
                }

                // Terminate iteration, when a ship has the position (-1/-1), as this means we're comparing against ships
                // which have not yet been placed.
                if ( aShip.XPos < 0 || aShip.YPos < 0 )
                {
                    break;
                }

                for ( int anIdx = 0; anIdx < aShip.Length; anIdx++ )
                {
                    if ( theShip.IsHorizontal() )
                    {
                        if ( aShip.IsHit( theShip.XPos + anIdx, theShip.YPos, true ) ) return true;
                    }
                    else
                    {
                        if ( aShip.IsHit( theShip.XPos, theShip.YPos + anIdx, true ) ) return true;
                    }
                }
            }

            return false;
        }

        private void placeShipsRandomly()
        {
            Random aRng = new Random();

            foreach ( Ship aShip in m_Ships )
            {
                do
                {
                    // Generate a random direction in which to place the ship.
                    Orientation aDir = aRng.Next() % 2 == 0 ? Orientation.Horizontal : Orientation.Vertical;
                    // Generate a random value for the reversed state.
                    bool isReversed = (aRng.Next() % 2 == 0);

                    // Generate a random position, that is within the play area.
                    int anXPos = aRng.Next( 0, 10 - aShip.Length );
                    int anYPos = aRng.Next( 0, 10 );

                    // Swap the two position values, if the orientation is vertical.
                    (anXPos, anYPos) = (aDir == Orientation.Vertical) ? (anYPos, anXPos) : (anXPos, anYPos);

                    aShip.SetShipValues(anXPos, anYPos, aDir, isReversed);
                }
                while ( isColliding( aShip ) );
            }
        }

        private int convertLetterIndex( char theLetter )
        {
            int anIdx = theLetter;
            anIdx -= 65;
            anIdx = anIdx >= 32 ? anIdx - 32 : anIdx;
            return anIdx;
        }

        private bool tryGetPosition( string theTargetString, out (int, int) theResult )
        {
            theResult = (0, 0);

            if ( string.IsNullOrWhiteSpace( theTargetString ) )
            {
                return false;
            }

            if ( theTargetString.Length < 2 )
            {
                return false;
            }

            char aLetter = theTargetString[0];
            string aNumber = theTargetString.Substring( 1 );

            if ( !char.IsLetter( aLetter ) )
            {
                // Maybe raise exception here.
                return false;
            }

            if ( !int.TryParse( aNumber, out int aValue ) )
            {
                return false;
            }

            int anXPos = convertLetterIndex( aLetter );
            theResult = (anXPos, aValue - 1);

            return true;
        }

        private bool isValidPosition( (int, int) thePosition )
        {
            return ( thePosition.Item1 >= 0 && thePosition.Item1 < ShipGridModel.GRID_SIZE && thePosition.Item2 >= 0 && thePosition.Item2 < ShipGridModel.GRID_SIZE );
        }

        private void drawShip(Ship aShip)
        {
            for (int anIdx = 0; anIdx < aShip.Length; anIdx++ )
            {
                if (aShip.IsHorizontal())
                {
                    m_ShipGrid.SetCell(aShip.XPos + anIdx, aShip.YPos, aShip.Letter);
                }
                else
                {
                    m_ShipGrid.SetCell(aShip.XPos, aShip.YPos + anIdx, aShip.Letter);
                }
            }
        }

        private void drawShipSunk(Ship theShip)
        {
            drawShip( theShip );
        }

        public void ProcessShot( string theTargetString )
        {
            if ( !tryGetPosition( theTargetString, out (int, int) thePosition ) )
            {
                // Maybe raise exception here.
                return;
            }

            if ( !isValidPosition( thePosition ) )
            {
                // Maybe raise exception here.
                return;
            }

            int anXPos = thePosition.Item1;
            int anYPos = thePosition.Item2;

            foreach (Ship aShip in m_Ships)
            {
                if ( aShip.IsHit( anXPos, anYPos ) )
                {
                    if ( aShip.IsSunk() )
                    {
                        drawShipSunk( aShip );
                        return;
                    }

                    m_ShipGrid.SetCell( anXPos, anYPos, 'h' );
                    return;
                }
            }

            m_ShipGrid.SetCell( anXPos, anYPos, 'm' );
        }

        public void DrawAllShips()
        {
            init();

            foreach ( var aShip in m_Ships )
            {
                drawShip(aShip);
            }
        }
    }
}
