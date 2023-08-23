using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Controls;

namespace Battleship_v2.Models
{
    public class ShipGridModel : PropertyChangeHandler
    {
        public const int GRID_SIZE = 10;

        // A seed for the Ship Randomizer, that will be modified based on ownership of the grid.
        // This is here to avoid the problem of both players having the same ship layout,
        // since the default seed for the Random class is the time, which both instances of this class use.
        // therefore creating this race condition.
        private int m_RandomSeed = (int)DateTime.UtcNow.Ticks;

        public ShipGridViewModel ViewModel { get; private set; }

        // This keeps track, if the Grid is owned by the player or the opponent.
        public ShipGridModel(ShipGridViewModel theViewModel, bool isOwn)
        {
            ViewModel = theViewModel;
            //GameManagerService.Instance.InjectShipGridModel( this );

            // Set the random seed based on grid owner, to avoid both sides having an identical grid.
            m_RandomSeed = isOwn ? m_RandomSeed : m_RandomSeed << 1;

            ViewModel.Grid = new DataTable();

            char theColumnLetter = 'A';

            // Generate Columns
            for (int anIdx = 0; anIdx < 11; anIdx++)
            {
                var aCol = new DataColumn();

                if (anIdx == 0)
                {
                    aCol.ColumnName = "#";
                    /*aCol.AutoIncrement = true;
                    aCol.AutoIncrementSeed = 1;*/
                }
                else
                {
                    aCol.ColumnName = $"{theColumnLetter++}";
                    aCol.DefaultValue = "w";
                }
                ViewModel.Grid.Columns.Add(aCol);
            }

            // Generate Rows
            for (int anIdx = 0; anIdx < 10; anIdx++)
            {
                DataRow aRow = ViewModel.Grid.NewRow();
                aRow[0] = anIdx + 1;
                ViewModel.Grid.Rows.Add(aRow);
            }

            ViewModel.Ships = generateShipList();
            //DrawAllShips();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Ship> generateShipList()
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
            placeShipsRandomly(aList);

            return aList;
        }

        private bool isInBounds(int theXPos, int theYPos)
        {
            // In case a change breaks hitting any shot in the last column, it's probably due this line.
            // The X-Position bounds check might need to be adjusted. At the moment we're pretending that the first column i.e.
            // the one containing the row number, does not exist. This might not always be possible in the future.
            // Should that be the case, change the bounds check for "theXPos" to the range [1,10] or [1,11).
            return (theXPos >= 0 && theXPos < GRID_SIZE && theYPos >= 0 && theYPos < GRID_SIZE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        /// <param name="theValue"></param>
        public void SetCell(int theXPos, int theYPos, object theValue)
        {
            if (!isInBounds(theXPos, theYPos))
            {
                return;
            }

            // We need to increment here, since Column zero contains the row number
            // and when we say theXPos = 0, we refer to the first column of the playing field.
            theXPos++;

            ViewModel.Grid.Rows[theYPos][theXPos] = $"{theValue}";
            ViewModel.Grid.AcceptChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thePosition"></param>
        /// <param name="theValue"></param>
        public void SetCell(Position thePosition, object theValue)
        {
            SetCell(thePosition.X, thePosition.Y, theValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        /// <returns></returns>
        public object GetCell(int theXPos, int theYPos)
        {
            if (isInBounds(theXPos, theYPos))
            {
                return '\0';
            }

            var aRow = ViewModel.Grid.Rows[theYPos];
            var aCell = aRow[ViewModel.Grid.Columns[theXPos]];
            return ((string)aCell)[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thePosition"></param>
        /// <returns></returns>
        public object GetCell(Position thePosition)
        {
            return GetCell(thePosition.X, thePosition.Y);
        }

        /// <summary>
        /// Draws all Ships onto the Grip.
        /// </summary>
        public void DrawAllShips()
        {
            foreach (var aShip in ViewModel.Ships)
            {
                DrawShip(aShip);
                aShip.Cells.ForEach(cell => Debug.WriteLine(cell));
                Debug.WriteLine("");
            }
        }

        /// <summary>
        /// Draws the Ship onto the Grid.
        /// </summary>
        /// <param name="theShip"></param>
        public void DrawShip(Ship theShip, bool isSunk = true)
        {
            foreach (var aCell in theShip.Cells)
            {
                SetCell(aCell, theShip.Letter);
            }

            if (!isSunk) return;

            ViewModel.Ships.Remove(theShip);

            // This is just here to prove a point.
            if (ViewModel.Ships.Count == 0)
            {
                WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameOverViewModel(GameManagerService.Instance.YourTurn);
            }
        }

        /// <summary>
        /// Checks whether or not the ship is colliding with any other ship.
        /// </summary>
        /// <param name="theShip"></param>
        /// <param name="theShipList"></param>
        /// <returns></returns>
        private bool isColliding(Ship theShip, List<Ship> theShipList)
        {
            foreach (Ship aShip in theShipList)
            {
                // Skip the iteration, when comparing against itself.
                if (aShip == theShip) break;

                // Check if the two ships share any position.
                if (aShip.IntersectsWith(theShip)) return true;
            }

            return false;
        }

        /// <summary>
        /// This will generate a random position for all ships, which does not collide with any other ship.
        /// The input list will be modified.
        /// </summary>
        /// <param name="theShipList"></param>
        private void placeShipsRandomly(List<Ship> theShipList)
        {
            Random aRng = new Random(m_RandomSeed);
            Position aPos = new Position();

            // Iterate through all Ships in the List to place them.
            foreach (Ship aShip in theShipList)
            {
                // Repeat the Placement until a position is found, where the ship does not collide with any other ship.
                do
                {
                    // Generate a random direction in which to place the ship.
                    Orientation aDir = aRng.Next() % 2 == 0 ? Orientation.Horizontal : Orientation.Vertical;
                    // Generate a random value for the reversed state.
                    bool isReversed = (aRng.Next() % 2 == 0);

                    // Generate a random position, that is within the play area.
                    // 10 - aShip.Length + 1, because the upper bound is exclusive and a ship with a length of two cells.
                    // Should at most be placed on X-Postion 8 due to the location of a ship being it's top left corner.
                    aPos.X = aRng.Next(10 - aShip.Length + 1);
                    aPos.Y = aRng.Next(10);

                    // Swap the two Position values, if the ship aligned vertically.
                    if (aDir == Orientation.Vertical) aPos.Swap();

                    aShip.SetShipValues(aPos, aDir, isReversed);
                }
                while (isColliding(aShip, theShipList));
            }
        }
    }
}