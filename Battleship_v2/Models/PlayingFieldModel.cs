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

        public PlayingFieldViewModel ViewModel { get; private set; }

        // This keeps track, if the Grid is owned by the player or the opponent.
        public PlayingFieldModel(PlayingFieldViewModel theViewModel, bool isOwn, List<ushort> theShipList)
        {
            ViewModel = theViewModel;

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
    }
}