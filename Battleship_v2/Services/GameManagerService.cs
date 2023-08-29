using Battleship_v2.Enemies;
using Battleship_v2.Models;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using System;
using System.Diagnostics;

namespace Battleship_v2.Services
{
    /// <summary>
    /// An enum that represents the owner of the current turn.
    /// </summary>
    public enum PlayerType
    {
        You = 0,
        Enemy = 1,
    }

    public class GameManagerService : PropertyChangeHandler
    {
        /// <summary>
        /// An instance of a randomizer shared across the whole class.
        /// </summary>
        private static Random aRng = new Random();

        /// <summary>
        /// The Opponent object, this can either be an AI or another human player.
        /// </summary>
        public Enemy Opponent { get; private set; }

        /// <summary>
        /// The size of one of the ship grid sides, it's valid for both of them, since the grids are square.
        /// </summary>
        public const int GRID_SIZE = 10;

        /// <summary>
        /// Keeps track of whose turn it is.
        /// </summary>
        public PlayerType CurrentTurn
        {
            get => m_CurrentTurn;
            set
            {
                if (m_CurrentTurn == value) return;
                m_CurrentTurn = value;
                if (m_CurrentTurn == PlayerType.Enemy)
                {
                    PlayNextMove();
                }
                NotifyPropertyChanged(nameof(CurrentTurn));
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
        public GameManagerService InjectShipGridModel(ShipGridModel theModel, bool isOwnGrid = true)
        {
            if (isOwnGrid)
            {
                OwnGrid = theModel;
            }
            else
            {
                EnemyGrid = theModel;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theModel"></param>
        /// <returns></returns>
        public GameManagerService InjectTargetInputModel(TargetInputModel theModel)
        {
            TargetInput = theModel;
            return this;
        }

        /// <summary>
        /// The Instance Property and the only way to access this class, as it is a singleton.
        /// </summary>
        public static GameManagerService Instance { get; private set; } = new GameManagerService();

        /// <summary>
        /// The private constructor of the class, only to be called once on instantiation.
        /// </summary>
        private GameManagerService() { }

        /// <summary>
        /// Converts any letter into an integer beginning at 0 disregarding case.
        /// </summary>
        /// <param name="theLetter">The letter to be converted</param>
        /// <returns></returns>
        private int convertLetterIndex(char theLetter)
        {
            // Implicitly casting the char to an integer.
            int anIdx = theLetter;
            // Subtracting 65 to make 'A' == 0
            anIdx -= 65;
            // In case the value is still bigger than 32 (indicating it's a lowercase letter) subtract 32 to make 'a' == 0
            anIdx = anIdx >= 32 ? anIdx - 32 : anIdx;
            return anIdx;
        }

        /// <summary>
        /// The logic for converting a target string into an X,Y integer position.
        /// </summary>
        /// <param name="theTargetString">the target string to parse</param>
        /// <param name="theMove">the Move object to be written into</param>
        /// <returns></returns>
        private bool tryGetPosition(string theTargetString, out Position theMove)
        {
            theMove = null;

            if (string.IsNullOrWhiteSpace(theTargetString))
            {
                return false;
            }

            if (theTargetString.Length < 2)
            {
                return false;
            }

            char aLetter = theTargetString[0];
            string aNumber = theTargetString.Substring(1);

            if (!char.IsLetter(aLetter))
            {
                // Maybe raise exception here.
                return false;
            }

            if (!int.TryParse(aNumber, out int aValue))
            {
                return false;
            }

            /*theMove.X = convertLetterIndex( aLetter );
            theMove.Y = aValue - 1;*/

            theMove = new Position(convertLetterIndex(aLetter), aValue - 1);

            return true;
        }

        /// <summary>
        /// This Method handles the shot processing, when you yourself fire.
        /// </summary>
        /// <param name="theTargetString"></param>
        public void ProcessShot(string theTargetString)
        {
            // Parse out the target position.
            if (!tryGetPosition(theTargetString, out Position aMove))
            {
                return;
            }

            // Play the move.
            processShot(aMove, EnemyGrid);

            if (Opponent is EnemyPerson)
            {
                NetworkService.Instance.NetworkPeer.SendMessage(aMove.ToMessage());
            }
        }

        /// <summary>
        /// This processes any given shot and encapsulates the main game logic.
        /// </summary>
        /// <param name="theMove">The move to play</param>
        /// <param name="theGrid">The Board to make the move on</param>
        private void processShot(Position theMove, ShipGridModel theGrid)
        {
            // Checks if the move is valid, if not the turn is over without playing a move.
            if (!theMove.IsValid())
            {
                // We change the turn here.
                changeTurns();
                return;
            }

            // Iterating through all ships not yet sunk.
            foreach (Ship aShip in theGrid.ViewModel.Ships)
            {
                // Check to see, if the move is a hit on the current ship.
                if (aShip.IsHit(theMove))
                {
                    // Check if that hit caused the ship to sink.
                    if (aShip.IsSunk())
                    {
                        // Draw the Ship, when sunk
                        theGrid.DrawShip(aShip);
                    }
                    else
                    {
                        // Mark the shot as a hit, in the case that the ship wasn't sunk
                        theGrid.SetCell(theMove.X, theMove.Y, 'h');
                    }

                    // Change turns.
                    changeTurns();
                    return;
                }
            }

            // this is the base case, if there was no hit on the cell, we mark it as a miss.
            theGrid.SetCell(theMove.X, theMove.Y, 'm');

            // When you made your move change the turn to your opponent.
            changeTurns();
        }

        /// <summary>
        /// Inverts the current turn. This means converting YourTurn => EnemyTurn or EnemyTurn => YourTurn
        /// </summary>
        private void changeTurns()
        {
            CurrentTurn = CurrentTurn == PlayerType.Enemy ? PlayerType.You : PlayerType.Enemy;
        }

        /// <summary>
        /// This will instantiate an opponent type, based on the passed in enum value.
        /// </summary>
        /// <param name="theDifficutly">This is an enum value, indicating the opponent type.</param>
        public void SelectDifficulty(Difficulty theDifficutly)
        {
            switch (theDifficutly)
            {
                case Difficulty.Easy:
                    Opponent = new EnemyEasy();
                    break;
                case Difficulty.Medium:
                    Opponent = new EnemyMedium();
                    break;
                case Difficulty.Hard:
                    Opponent = new EnemyHard();
                    break;
                case Difficulty.Person:
                    Opponent = new EnemyPerson();
                    break;
            }
        }

        /// <summary>
        /// This Method is called, when you're waiting for your opponent to make a move.
        /// </summary>
        public void PlayNextMove(Position theNextMove = null)
        {
            // If there was no move passed in as an argument, try to get it.
            if (theNextMove is null)
            {
                // Retrieves the next move to play, from either the AI or the opposing player.
                theNextMove = Opponent.NextMove();
            }

            // This is needed, when playing against other people, since in the case of them not submitting a move instantanious
            // aNextMove will be none.
            if (theNextMove is null)
            {
                return;
            }

            // This will process the shot, and encapsulates most of the game logic.
            processShot(theNextMove, OwnGrid);
            // For good measure.
            Debug.WriteLine($"Made my move: {theNextMove}");
        }
    }
}
