using System.Collections.Generic;
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

            };
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
