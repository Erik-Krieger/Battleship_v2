using System;

namespace Battleship_v2.Items
{
    public class ShipGridCell
    {
        private char m_Letter = 'w';

        public string Letter
        {
            get => m_Letter.ToString();
            set => m_Letter = value[0];
        }

        private ConsoleColor m_Color = ConsoleColor.Green;

        public ConsoleColor Color
        {
            get => m_Color;
            set => m_Color = value;
        }

        public ShipGridCell() { }
    }
}
