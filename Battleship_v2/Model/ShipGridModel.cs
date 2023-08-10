using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using Battleship_v2.Ships;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class ShipGridModel
    {
        private ShipGridViewModel m_ViewModel;
        private TargetInputViewModel m_TargetInputViewModel;
        private List<Ship> m_Ships;

        public ShipGridModel(ShipGridViewModel theViewModel)
        {
            m_ViewModel = theViewModel;

            m_Ships = new List<Ship>()
            {
                new Carrier(),
                new Battleship(),
                new Battleship(),
                new Destroyer(),
                new Destroyer(),
                new Submarine(),
                new Submarine(),
                new PatrolBoat(),
                new PatrolBoat(),
                new PatrolBoat()
            };

            placeShipsRandomly();
            drawAllShips();
        }

        private void placeShipsRandomly()
        {
            Random aRng = new Random();

            foreach (var aShip in m_Ships)
            {
                do
                {
                    int anXPos = aRng.Next( 0, 10 - aShip.Length );
                    int anYPos = aRng.Next( 0, 10 );
                    var anOrientation = aRng.Next() % 2 == 0 ? Orientation.Horizontal : Orientation.Vertical;
                    bool isReversed = aRng.Next() % 2 == 0;

                    if ( anOrientation == Orientation.Vertical )
                    {
                        (anXPos, anYPos) = (anYPos, anXPos);
                    }

                    aShip.SetShipValues(anXPos, anYPos, anOrientation, isReversed);
                }
                while (collidesWithAnotherShip(aShip));
            }
        }

        private void drawAllShips()
        {
            foreach ( var aShip in m_Ships )
            {
                if ( aShip.IsHorizontal() )
                {
                    for ( int anIdx = 0; anIdx < aShip.Length; anIdx++ )
                    {
                        m_ViewModel.SetCell( aShip.XPos + anIdx, aShip.YPos, aShip.Letter );
                    }
                }
                else
                {
                    for ( int anIdx = 0; anIdx < aShip.Length; anIdx++ )
                    {
                        m_ViewModel.SetCell( aShip.XPos, aShip.YPos + anIdx, aShip.Letter );
                    }
                }
            }
        }

        private bool collidesWithAnotherShip(Ship theShip)
        {
            bool isColliding = false;
            
            foreach (var aShip in m_Ships)
            {
                // Skip the Iteration, when comparing against itself.
                if (aShip == theShip)
                {
                    continue;
                }

                if (aShip.NotPlaced())
                {
                    break;
                }

                if (theShip.IsHorizontal())
                {
                    for (int anIdx = 0; anIdx < theShip.Length; anIdx++ )
                    {
                        if (aShip.IsHit(theShip.XPos + anIdx, theShip.YPos, true))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int anIdx = 0; anIdx < theShip.Length; anIdx++ )
                    {
                        if (aShip.IsHit(theShip.XPos, theShip.YPos + anIdx, true))
                        {
                            return false;
                        }
                    }
                }
            }

            return isColliding;
        }

        public bool ProcessShot( string theInputString )
        {
            if ( theInputString == null || theInputString.Length < 2 )
            {
                return false;
            }

            char aLetter = theInputString[0];
            string aNumber = theInputString.Substring( 1 );

            if ( !char.IsLetter( aLetter ) )
            {
                return false;
            }

            if ( !int.TryParse( aNumber, out int theRowNumber ) )
            {
                return false;
            }

            int anXPos = convertLetterToIndex( aLetter );
            int anYPos = theRowNumber - 1;

            if ( anXPos < 0 )
            {
                return false;
            }

            return true;
        }

        private static int convertLetterToIndex( char theLetter )
        {
            if ( !char.IsLetter( theLetter ) )
            {
                return -1;
            }

            if ( char.IsUpper( theLetter ) )
            {
                return theLetter - 65;
            }
            else
            {
                return theLetter - 97;
            }
        }
    }
}
