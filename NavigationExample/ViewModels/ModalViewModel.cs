using NavigationExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ModalViewModel(string parameter)
        {
            BindingText = parameter;
        }

        public void CloseWindow()
        {
            NavigationService.Close(this);
        }
    }
}
