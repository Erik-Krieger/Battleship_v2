using System;
using System.Windows.Input;

namespace Battleship_v2
{
    public class CommandHandler : ICommand
    {
        private Action m_Action;
        private Func<bool> m_CanExecute;

        /// <summary>
        /// Creates instance of the command handler
        /// </summary>
        /// <param name="action">Action to be executed by the command</param>
        /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
        public CommandHandler( Action theAction, Func<bool> canExecute = null )
        {
            m_Action = theAction;
            m_CanExecute = canExecute;
        }

        /// <summary>
        /// Wires CanExecuteChanged event 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forcess checking if execute is allowed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute( object parameter )
        {
            return m_CanExecute == null ? true : m_CanExecute.Invoke();
        }

        public void Execute( object parameter )
        {
            m_Action();
        }
    }
}