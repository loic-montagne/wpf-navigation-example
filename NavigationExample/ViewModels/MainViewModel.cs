using NavigationExample.Services;
using NavigationExample.Views;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand ShowModalWindowCommand => new Command.RelayCommand(ShowModalWindow);
        public ICommand ShowNonModalWindowCommand => new Command.RelayCommand(ShowNonModalWindow);
        public ICommand ShowPagesNavigationWindowCommand => new Command.RelayCommand(ShowPagesNavigationWindow);

        public MainViewModel(NavigationService navigation)
            : base (navigation)
        { }

        public void ShowModalWindow()
        {
            Navigation.ShowDialog<ModalWindow, ModalViewModel>("Un texte issu du constructeur du viewmodel pour la fenêtre modale.");
        }

        public void ShowNonModalWindow()
        {
            Navigation.Show<NonModalWindow, NonModalViewModel>();
        }

        public void ShowPagesNavigationWindow()
        {
            Navigation.Show<PagesNavigationWindow, PagesNavigationViewModel>();
        }

    }
}
