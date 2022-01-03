using NavigationExample.Services;
using System;
using System.Windows.Input;

namespace NavigationExample.ViewModels
{
    public class PagesNavigationViewModel : BaseViewModel
    {
        public Type CurrentPageViewModelType 
        { 
            get => GetProperty<Type>();
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
            CurrentPageViewModelType = typeof(Page1ViewModel);
        }
        public void ShowPage2()
        {
            CurrentPageViewModelType = typeof(Page2ViewModel);
        }
        public void CloseWindow()
        {
            Navigation.Close(this);
        }
    }
}
