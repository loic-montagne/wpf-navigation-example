using NavigationExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public NonModalViewModel()
        {
            BindingText = "Un texte issu du databinding pour la fenêtre non modale.";
        }

        public void CloseWindow()
        {
            NavigationService.Close(this);
        }
    }
}
