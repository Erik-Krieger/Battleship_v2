using System;
using System.Diagnostics;
using Battleship_v2.Enemies;
using Battleship_v2.Models;
using Battleship_v2.Ships;
using Battleship_v2.Utility;

namespace Battleship_v2.Services
{
    public enum PlayerType
    {
        You = 0,
        Enemy = 1,
    }

    public class GameManagerService
    {
        private static Random aRng = new Random();
        private Enemy m_Enemy;

        public const int GRID_SIZE = 10;

        public PlayerType CurrentTurn
        {
            get => m_CurrentTurn;
            set
            {
                if  (m_CurrentTurn == value) return;
                m_CurrentTurn = value;
                if (m_CurrentTurn == PlayerType.Enemy)
                {
                    playMove();
                }
            }
        }
        private PlayerType m_CurrentTurn = PlayerType.You; //= aRng.Next() % 2 == 0 ? PlayerType.You : PlayerType.Enemy;

        public bool YourTurn { get => CurrentTurn == PlayerType.You; }

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

        private bool tryGetPosition( string theTargetString, out Position theMove )
        {
            theMove = new Position();

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

            theMove.X = convertLetterIndex( aLetter );
            theMove.Y = aValue;

            return true;
        }

        /// <summary>
        /// This Method handles the shot processing, when you yourself fire.
        /// </summary>
        /// <param name="theTargetString"></param>
        /// <param name="theGrid"></param>
        public void ProcessShot( string theTargetString )
        {
            if ( !tryGetPosition( theTargetString, out Position theMove ) )
            {
                // Maybe raise exception here.
                return;
            }

            processShot( theMove, EnemyGrid);
        }

        private void processShot(Position theMove, ShipGridModel theGrid)
        {
            if ( !theMove.IsValid() )
            {
                // Maybe raise exception here.
                return;
            }

            foreach ( Ship aShip in EnemyGrid.ViewModel.Ships )
            {
                Debug.WriteLine( $"{aShip.HitCount}" );
                if ( aShip.IsHit( theMove ) )
                {
                    if ( aShip.IsSunk() )
                    {
                        // Draw the Ship, when sunk
                        EnemyGrid.DrawShip( aShip );
                        return;
                    }

                    EnemyGrid.SetCell( theMove.X, theMove.Y, 'h' );
                    return;
                }
            }

            EnemyGrid.SetCell( theMove.X, theMove.Y, 'm' );
            // When you made your move change the turn to your opponent.
            changeTurns();
        }

        private void changeTurns()
        {
            CurrentTurn = CurrentTurn == PlayerType.Enemy ? PlayerType.You : PlayerType.Enemy;
        }

        public void SelectDifficulty( Difficulty theDifficutly )
        {
            switch ( theDifficutly )
            {
                case Difficulty.Easy:
                    m_Enemy = new EnemyEasy();
                    break;
                case Difficulty.Medium:
                    m_Enemy = new EnemyMedium();
                    break;
                case Difficulty.Hard:
                    m_Enemy = new EnemyHard();
                    break;
                case Difficulty.Person:
                    m_Enemy = new EnemyPerson();
                    break;
            }
        }

        private void playMove()
        {
            Position aNextMove = m_Enemy.NextMove();
            processShot(aNextMove, OwnGrid);
        }
    }
}
