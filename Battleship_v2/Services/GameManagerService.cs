using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using Battleship_v2.Models;
using Battleship_v2.Ships;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Services
{
    public enum PlayerType
    {
        You = 0,
        Enemy = 1,
        Pc = 2,
    }

    public class GameManagerService
    {
        public const int GRID_SIZE = 10;

        public ShipGridModel OwnGrid { get; set; }
        public ShipGridModel EnemyGrid { get; set; }
        public TargetInputModel TargetInput { get; set; }

        /// <summary>
        /// This is used to inject the ShipGridModel dependency
        /// </summary>
        /// <param name="theModel"></param>
        /// <param name="isOwnGrid"></param>
        /// <returns></returns>
        public GameManagerService InjectShipGridModel( ShipGridModel theModel, bool isOwnGrid = true )
        {
            if ( isOwnGrid )
            {
                OwnGrid = theModel;
            }
            else
            {
                EnemyGrid = theModel;
            }
            return this;
        }

        public GameManagerService InjectTargetInputModel( TargetInputModel theModel )
        {
            TargetInput = theModel;
            return this;
        }

        public static GameManagerService Instance { get; private set; } = new GameManagerService();
        private GameManagerService() { }

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

            foreach (Ship aShip in EnemyGrid.ViewModel.Ships)
            {
                Debug.WriteLine($"{aShip.HitCount}");
                if ( aShip.IsHit( anXPos, anYPos ) )
                {
                    if ( aShip.IsSunk() )
                    {
                        // Draw the Ship, when sunk
                        EnemyGrid.DrawShip( aShip );
                        return;
                    }

                    EnemyGrid.SetCell( anXPos, anYPos, 'h' );
                    return;
                }
            }

            EnemyGrid.SetCell( anXPos, anYPos, 'm' );
        }
        
    }
}
