using NavigationExample.Services;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class PagesNavigationViewModel : BaseViewModel
    {
        public BaseViewModel CurrentPageViewModel 
        { 
            get => GetProperty<BaseViewModel>();
            set => SetProperty(value);
        }
        public ICommand ShowPage1Command => new Command.RelayCommand(ShowPage1);
        public ICommand ShowPage2Command => new Command.RelayCommand(ShowPage2);
        public ICommand CloseWindowCommand => new Command.RelayCommand(CloseWindow);

        public PagesNavigationViewModel(NavigationService navigation) : base(navigation)
        {
            ShowPage1();
        }

        public void ShowPage1()
        {
            CurrentPageViewModel = Navigation.GetViewModelInstance<Page1ViewModel>();
        }
        public void ShowPage2()
        {
            CurrentPageViewModel = Navigation.GetViewModelInstance<Page2ViewModel>();
        }
        public void CloseWindow()
        {
            Navigation.Close(this);
        }
    }
}
