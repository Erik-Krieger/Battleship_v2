using Battleship_v2.Enemies;
using Battleship_v2.Models;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

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
        /// A seed for the random generator, that is by default set to the current UTC time in ms.
        /// </summary>
        private static int m_RandomSeed = 17;//DateTime.UtcNow.Millisecond;

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
            set => SetProperty(ref m_CurrentTurn, value);
        }
        private PlayerType m_CurrentTurn = PlayerType.You; //= aRng.Next() % 2 == 0 ? PlayerType.You : PlayerType.Enemy;

        public bool YourTurn { get => CurrentTurn == PlayerType.You; }

        public PlayingFieldModel OwnGrid { get; set; }
        public PlayingFieldModel EnemyGrid { get; set; }
        public TargetInputModel TargetInput { get; set; }

        /// <summary>
        /// This is used to inject the ShipGridModel dependency
        /// </summary>
        /// <param name="theModel"></param>
        /// <param name="isOwnGrid"></param>
        /// <returns></returns>
        public GameManagerService InjectShipGridModel(PlayingFieldModel theModel, bool isOwnGrid = true)
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
        /// Picks who goes first a random
        /// </summary>
        /// <returns></returns>
        public PlayerType SetFirstTurnRandom()
        {
            /*CurrentTurn = aRng.Next() % 2 == 0 ? PlayerType.You : PlayerType.Enemy;
            return CurrentTurn;*/
            CurrentTurn = PlayerType.You;
            return CurrentTurn;
        }

        public void SetFirstTurnFromInt(int thePlayerType)
        {
            if (thePlayerType == (int)PlayerType.You)
            {
                CurrentTurn = PlayerType.You;
            }
            else if (thePlayerType == (int)PlayerType.Enemy)
            {
                CurrentTurn = PlayerType.Enemy;
            }
            else
            {
                throw new ArgumentException($"The value of the playerType can only be 0 or 1, {thePlayerType} is an invalid input", nameof(thePlayerType));
            }
        }

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
        /// It takes a string in the format <Letter><Number>
        /// where the Letter has to be in the range [A-J] and the number in the range [1-10]
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
        /// This Method handles the shot processing, when you yourself fire.
        /// It takes to integers indicating the X- and Y-Position of the shot.
        /// where the values have to be in the range [0-9].
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        public void ProcessShot(int theXPos, int theYPos)
        {
            Position aMove = new Position(theXPos, theYPos);

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
        private void processShot(Position theMove, PlayingFieldModel theGrid)
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
                HitType aHitType = aShip.IsHit(theMove);

                // Check to see, if the move is a hit on the current ship.
                if (aHitType == HitType.Hit)
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
                else if (aHitType == HitType.Repeat)
                {
                    // This will internally call the underlying DataTable and force in to update
                    // even though nothing has actually changed.
                    // This is needed to fix a weird bug, where when repeatedly clicking a cell that was marked as a hit before,
                    // the game wouldn't proceed after the second click on that cell until you click anyother cell.
                    // And for some reason, I do not even begin to grasp, this line fixes that.
                    // WPF is definitely weird.
                    theGrid.AcceptChanges();

                    // Change turns
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

            if (CurrentTurn == PlayerType.Enemy)
            {
                PlayNextMove();
            }
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
            //Debug.WriteLine($"Made my move: {theNextMove}");
        }

        /// <summary>
        /// Generates a List of all the ships of this grid and places them.
        /// </summary>
        /// <returns></returns>
        public List<Ship> GenerateShipList(List<ushort> theShipList = null)
        {
            // Create a list of all the ships, that will be on this grid.
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

            // Check if there was a list of ships passed in.
            // This is the case, when we're connected to a multiplayer game as the client.
            if (!(theShipList is null) && theShipList.Count == aList.Count)
            {
                // Iterate through the ships and set their values from the bitFields.
                for (int anIdx = 0; anIdx < aList.Count; anIdx++)
                {
                    Debug.WriteLine($"{anIdx + 1}: {theShipList[anIdx]}");
                    aList[anIdx].FromBitField(theShipList[anIdx]);
                }

                return aList;
            }

            // This will do an in place modification of the positions.
            placeShipsRandomly(aList);

            return aList;
        }

        /// <summary>
        /// This will generate a random position for all ships, which does not collide with any other ship.
        /// The input list will be modified.
        /// </summary>
        /// <param name="theShipList"></param>
        private void placeShipsRandomly(List<Ship> theShipList)
        {
            Random aRng = new Random(m_RandomSeed++);
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
    }
}
