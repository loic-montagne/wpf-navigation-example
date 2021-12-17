using NavigationExample.Services;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class ModalViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand => new Command.RelayCommand(CloseWindow);
        public string BindingText
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public ModalViewModel(NavigationService navigation, string parameter)
            : base(navigation)
        {
            BindingText = parameter;
        }

        public void CloseWindow()
        {
            Navigation.Close(this);
        }
    }
}
