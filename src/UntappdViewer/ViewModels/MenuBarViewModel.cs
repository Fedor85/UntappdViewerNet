using System.Windows.Input;
using Prism.Commands;

namespace UntappdViewer.ViewModels
{
    public class MenuBarViewModel
    {
        public ICommand GoToWelcomeCommand { get; }

        public MenuBarViewModel()
        {
            GoToWelcomeCommand = new DelegateCommand(GoToWelcome);
        }

        private void GoToWelcome()
        {
            
        }
    }
}