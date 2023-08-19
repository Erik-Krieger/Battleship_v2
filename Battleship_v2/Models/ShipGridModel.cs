using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Controls;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class ShipGridModel
    {
        public const int GRID_SIZE = 10;

        public ShipGridViewModel ViewModel { get; private set; }

        public ShipGridModel( ShipGridViewModel theViewModel )
        {
            ViewModel = theViewModel;
            //GameManagerService.Instance.InjectShipGridModel( this );

            ViewModel.Grid = new DataTable();

            char theColumnLetter = 'A';

            // Generate Columns
            for ( int anIdx = 0; anIdx < 11; anIdx++ )
            {
                var aCol = new DataColumn();

                if ( anIdx == 0 )
                {
                    aCol.ColumnName = "#";
                    aCol.AutoIncrement = true;
                    aCol.AutoIncrementSeed = 1;
                }
                else
                {
                    aCol.ColumnName = $"{theColumnLetter++}";
                    aCol.DefaultValue = "w";
                }
                ViewModel.Grid.Columns.Add( aCol );
            }

            // Generate Rows
            for ( int anIdx = 0; anIdx < 10; anIdx++ )
            {
                DataRow aRow = ViewModel.Grid.NewRow();
                ViewModel.Grid.Rows.Add( aRow );
            }

            ViewModel.Ships = generateShipList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<Ship> generateShipList()
        {
            var aList = new List<Ship>()
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

            // This will do an in place modification of the positions.
            placeShipsRandomly( aList );

            return aList;
        }

        private bool isInBounds( int theXPos, int theYPos )
        {
            // In case a change breaks hitting any shot in the last column, it's probably due this line.
            // The X-Position bounds check might need to be adjusted. At the moment we're pretending that the first column i.e.
            // the one containing the row number, does not exist. This might not always be possible in the future.
            // Should that be the case, change the bounds check for "theXPos" to the range [1,10] or [1,11).
            return ( theXPos >= 0 && theXPos < GRID_SIZE && theYPos >= 0 && theYPos < GRID_SIZE );
        }

        public void SetCell( int theXPos, int theYPos, char theValue )
        {
            if ( !isInBounds( theXPos, theYPos ) )
            {
                return;
            }

            // We need to increment here, since Column zero contains the row number
            // and when we say theXPos = 0, we refer to the first column of the playing field.
            theXPos++;

            var aRow = ViewModel.Grid.Rows[theYPos];
            aRow[ViewModel.Grid.Columns[theXPos]] = $"{theValue}";
            ViewModel.UpdateGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        /// <returns></returns>
        public char GetCell( int theXPos, int theYPos )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return '\0';
            }

            var aRow = ViewModel.Grid.Rows[theYPos];
            var aCell = aRow[ViewModel.Grid.Columns[theXPos]];
            return ( (string)aCell )[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawAllShips()
        {
            foreach ( var aShip in ViewModel.Ships )
            {
                DrawShip( aShip );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aShip"></param>
        public void DrawShip( Ship aShip )
        {
            for ( int anIdx = 0; anIdx < aShip.Length; anIdx++ )
            {
                if ( aShip.IsHorizontal() )
                {
                    SetCell( aShip.XPos + anIdx, aShip.YPos, aShip.Letter );
                }
                else
                {
                    SetCell( aShip.XPos, aShip.YPos + anIdx, aShip.Letter );
                }
            }

            // This is just here to prove a point.
            if ( ViewModel.Ships.Count == 0)
            {
                if ( GameManagerService.Instance.YourTurn)
                {
                    Debug.WriteLine("You Won");
                }
                else
                {
                    Debug.WriteLine( "You Lost" );
                }
                throw new Exception("Game Over!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theShip"></param>
        /// <param name="theShipList"></param>
        /// <returns></returns>
        private static bool isColliding( Ship theShip, List<Ship> theShipList )
        {
            foreach ( Ship aShip in theShipList )
            {
                // Skip the iteration, when comparing against itself.
                if ( aShip == theShip )
                {
                    continue;
                }

                // Terminate iteration, when a ship has the position (-1/-1), as this means we're comparing against ships
                // which have not yet been placed.
                if ( aShip.NotPlaced() )
                {
                    break;
                }

                Position aPosition = aShip.Location.Clone();

                for ( int anIdx = 0; anIdx < aShip.Length; anIdx++ )
                {
                    if ( theShip.IsHorizontal() )
                    {
                        if ( theShip.IsHit( aPosition, true ) ) return true;
                        aPosition.MoveRight();
                    }
                    else
                    {
                        if ( theShip.IsHit( aPosition, true ) ) return true;
                        aPosition.MoveDown();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This will generate a random position for all ships, which does not collide with any other ship.
        /// The input list will be modified.
        /// </summary>
        /// <param name="theShipList"></param>
        private static void placeShipsRandomly( List<Ship> theShipList )
        {
            Random aRng = new Random();

            // Iterate through all Ships in the List to place them.
            foreach ( Ship aShip in theShipList )
            {
                Position aPos = new Position();

                // Repeat the Placement until a position is found, where the ship does not collide with any other ship.
                do
                {
                    // Generate a random direction in which to place the ship.
                    Orientation aDir = aRng.Next() % 2 == 0 ? Orientation.Horizontal : Orientation.Vertical;
                    // Generate a random value for the reversed state.
                    bool isReversed = ( aRng.Next() % 2 == 0 );

                    // Generate a random position, that is within the play area.
                    aPos.X = aRng.Next( 0, 10 - aShip.Length );
                    aPos.Y = aRng.Next( 0, 10 );

                    // Swap the two Position values, if the ship aligned vertically.
                    if ( aDir == Orientation.Vertical ) aPos.Swap();

                    aShip.SetShipValues( aPos, aDir, isReversed );
                }
                while ( isColliding( aShip, theShipList ) );
            }
        }

        /// <summary>
        /// This will return a List of all valid moves, but the implementation is terrible.
        /// </summary>
        /// <returns></returns>
        public List<Position> GetValidMoves()
        {
            List<Position> aMoveList = new List<Position>(100);

            for ( int aRow = 0; aRow < 10; aRow++ )
            {
                for ( int aCol = 0; aCol < 10; aCol++ )
                {
                    if ( GetCell( aCol, aRow ) == 'w' )
                    {
                        aMoveList.Add( new Position( aCol, aRow ) );
                    }
                }
            }

            return aMoveList;
        }
    }
}