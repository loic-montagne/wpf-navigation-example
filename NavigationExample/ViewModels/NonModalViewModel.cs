using NavigationExample.Services;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class NonModalViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand => new Command.RelayCommand(CloseWindow);
        public string BindingText
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public NonModalViewModel(NavigationService navigation)
            : base (navigation)
        {
            BindingText = "Un texte issu du databinding pour la fenêtre non modale.";
        }

        public void CloseWindow()
        {
            Navigation.Close(this);
        }
    }
}
