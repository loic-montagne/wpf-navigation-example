using NavigationExample.Services;
using NavigationExample.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand ShowModalWindowCommand => new Command.RelayCommand(ShowModalWindow);
        public ICommand ShowNonModalWindowCommand => new Command.RelayCommand(ShowNonModalWindow);

        public void ShowModalWindow()
        {
            NavigationService.ShowDialog<ModalWindow, ModalViewModel>("Un texte issu du constructeur du viewmodel pour la fenêtre modale.");
        }

        public void ShowNonModalWindow()
        {
            NavigationService.Show<NonModalWindow, NonModalViewModel>();
        }

    }
}
