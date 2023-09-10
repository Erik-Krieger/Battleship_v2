using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Battleship_v2.Models
{
    public class PlayingFieldModel : PropertyChangeHandler
    {
        public const int GRID_SIZE = 10;

        // A seed for the Ship Randomizer, that will be modified based on ownership of the grid.
        // This is here to avoid the problem of both players having the same ship layout,
        // since the default seed for the Random class is the time, which both instances of this class use.
        // therefore creating this race condition.
        private int m_RandomSeed = (int)DateTime.UtcNow.Ticks;

        private bool[,] m_CellsHit = new bool[GRID_SIZE, GRID_SIZE];

        public PlayingFieldViewModel ViewModel { get; private set; }

        // This keeps track, if the Grid is owned by the player or the opponent.
        public PlayingFieldModel(PlayingFieldViewModel theViewModel, bool isOwn, List<ushort> theShipList)
        {
            ViewModel = theViewModel;

            // Set the random seed based on grid owner, to avoid both sides having an identical grid.
            m_RandomSeed = isOwn ? m_RandomSeed : m_RandomSeed << 1;

            ViewModel.Ships = GameManagerService.Instance.GenerateShipList(theShipList);
            //DrawAllShips();
        }

        /// <summary>
        /// Checks if any given position is inside the playing area.
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        /// <returns></returns>
        private bool isInBounds(int theXPos, int theYPos)
        {
            // In case a change breaks hitting any shot in the last column, it's probably due this line.
            // The X-Position bounds check might need to be adjusted. At the moment we're pretending that the first column i.e.
            // the one containing the row number, does not exist. This might not always be possible in the future.
            // Should that be the case, change the bounds check for "theXPos" to the range [1,10] or [1,11).
            return (theXPos >= 0 && theXPos < GRID_SIZE && theYPos >= 0 && theYPos < GRID_SIZE);
        }

        /// <summary>
        /// This just calls the AcceptChanges Method on the DataTable.
        /// </summary>
        public void AcceptChanges()
        {
            ViewModel.Grid.AcceptChanges();
        }

        /// <summary>
        /// Sets the specified cell on the grid to the specified value
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

            if (m_CellsHit[theXPos, theYPos] == true && theValue == TileService.GetTile(TileType.Miss))
            {
                return;
            }

            m_CellsHit[theXPos, theYPos] = true;

            // We need to increment here, since Column zero contains the row number
            // and when we say theXPos = 0, we refer to the first column of the playing field.
            //theXPos++;

            ViewModel.Grid.Rows[theYPos][theXPos] = theValue;
            ViewModel.Grid.AcceptChanges();

            Debug.WriteLine(ViewModel.Grid);
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
        public void DrawAllShips(bool allSunk = false)
        {
            foreach (var aShip in ViewModel.Ships)
            {
                DrawShip(aShip, allSunk);
                Debug.WriteLine(aShip);
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
                SetCell(aCell, theShip.TileSprite);
            }

            if (!isSunk) return;

            ViewModel.Ships.Remove(theShip);

            // This is just here to prove a point.
            if (ViewModel.Ships.Count == 0)
            {   
                // Display the Won or lost screen depending on whose turn it currently is.
                WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameOverViewModel(GameManagerService.Instance.YourTurn);
            }
        }

        /// <summary>
        /// This is called, when a cell in the enemy grid is clicked.
        /// It takes an Integer as the first argument and an object as the second.
        /// These values are 1 based indeces.
        /// Meaning, they should be in the range from [1-10]
        /// </summary>
        /// <param name="theColumnIndex">An integer representing the column index</param>
        /// <param name="theRowIndex">An object, that is actually a string, that stores the row index</param>
        public void GridCellClicked(int theColumnIndex, int theRowIndex)
        {
            if (!GameManagerService.Instance.YourTurn) return;

            /*if (!int.TryParse(theRowIndex as string, out int aRowIndex))
            {
                return;
            }*/

            Debug.WriteLine($"Clicked at: {theColumnIndex}/{theRowIndex}");

            GameManagerService.Instance.ProcessShot(theColumnIndex - 1, theRowIndex - 1);
        }
    }
}