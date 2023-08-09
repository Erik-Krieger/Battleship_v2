using System;
using System.Windows.Input;

namespace Battleship_v2
{
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute( object parameter )
        {
            return CanExecuteAction( parameter );
        }

        public void Execute( object parameter )
        {
            ExecuteAction( parameter );
        }

        Func<object, bool> CanExecuteAction;
        Action<object> ExecuteAction;

        /// <summary>
        /// submits commands to the commandhandler
        /// </summary>
        /// <param name="theCanExecuteAction">the condition when methods are executed</param>
        /// <param name="theExecuteAction">the action the method will execute</param>        
        public CommandHandler( Func<object, bool> theCanExecuteAction, Action<object> theExecuteAction )
        {
            CanExecuteAction = theCanExecuteAction;
            ExecuteAction = theExecuteAction;
        }

        /// <summary>
        /// currently empty constructor for the commandhandler
        /// </summary>
        public CommandHandler() { }

        /// <summary>
        /// checks for changes and raises the event
        /// </summary>
        public void RaiseEvent()
        {
            CanExecuteChanged?.Invoke( this, EventArgs.Empty );
        }
    }
}