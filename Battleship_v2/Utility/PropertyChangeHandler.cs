using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Battleship_v2.Utility
{
    public class PropertyChangeHandler : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged( string thePropertyName ) => PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( thePropertyName ) );

        protected bool SetProperty<T>( ref T theField, T theValue, [CallerMemberName] string thePropertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals( theField, theValue ) ) return false;

            theField = theValue;
            NotifyPropertyChanged( thePropertyName );
            return true;
        }

        // props
        private string m_Name;
        public string Name
        {
            get => m_Name;
            set => SetProperty( ref m_Name, value, nameof( Name ) );
        }
    }
}
