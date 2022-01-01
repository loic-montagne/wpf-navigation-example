using Microsoft.Extensions.DependencyInjection;
using NavigationExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NavigationExample.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<object, Window> modalWindowsCache;
        private readonly Dictionary<object, Window> nonModalWindowsCache;
        private readonly Dictionary<BaseViewModel, Dictionary<Type, BaseViewModel>> pageViewModelsCache;

        public NavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            modalWindowsCache = new Dictionary<object, Window>();
            nonModalWindowsCache = new Dictionary<object, Window>();
            viewModelsCache = new Dictionary<Type, BaseViewModel>();
        }

        private TViewModel GetViewModelInstance<TViewModel>(params object[] viewModelParameters)
            where TViewModel : BaseViewModel
        {
            return (TViewModel)ActivatorUtilities.CreateInstance(serviceProvider, typeof(TViewModel), viewModelParameters);
        }

        private TView GetViewInstance<TView>(object viewModel)
            where TView : class
        {
            Type tView = typeof(TView);
            object view = Activator.CreateInstance(tView);
            var prop = tView.GetProperty("DataContext");
            prop?.SetValue(view, viewModel);
            return (TView)view;
        }

        public TView Show<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Window
            where TViewModel : BaseViewModel
        {
            var vm = GetViewModelInstance<TViewModel>(viewModelParameters);
            var view = GetViewInstance<TView>(vm);
            nonModalWindowsCache.Add(vm, view);
            view.Closing += (s, e) => 
            {
                if (s is Window w && nonModalWindowsCache.ContainsKey(w.DataContext))
                    nonModalWindowsCache.Remove(w.DataContext);
            };
            view.Show();
            return view;
        }

        public bool? ShowDialog<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Window
            where TViewModel : BaseViewModel
        {
            var vm = GetViewModelInstance<TViewModel>(viewModelParameters);
            var view = GetViewInstance<TView>(vm);
            modalWindowsCache.Add(vm, view);
            var result = view.ShowDialog();
            if (modalWindowsCache.ContainsKey(vm))
                modalWindowsCache.Remove(vm);
            return result;
        }

        public void Close(Window view, bool? result = null)
        {
            if (modalWindowsCache.ContainsKey(view.DataContext) && result != null)
                view.DialogResult = result;
            view.Close();
        }
        public bool Close<TViewModel>(TViewModel vm, bool? result = null)
            where TViewModel : BaseViewModel
        {
            if (nonModalWindowsCache.ContainsKey(vm))
            {
                Close(nonModalWindowsCache[vm], result);
                return true;
            }
            else if (modalWindowsCache.ContainsKey(vm))
            {
                Close(modalWindowsCache[vm], result);
                return true;
            }
            return false;
        }



        public TView GetPage<TView, TViewModel>(DataTemplateSelector templateSelector = null, params object[] viewModelParameters)
            where TView : Page
            where TViewModel : BaseViewModel
        {
            return GetViewInstance<TView>(
                GetViewModelInstance<TViewModel>(viewModelParameters));
        }

        public TViewModel GetPageViewModel<TViewModel>(params object[] viewModelParameters)
            where TViewModel : BaseViewModel
        {
            if (viewModelsCache.ContainsKey(typeof(TViewModel)))
                return (TViewModel)viewModelsCache[typeof(TViewModel)];
            return GetViewModelInstance<TViewModel>(viewModelParameters);
        }
        internal void ClosePageViewModel<TViewModel>()
            where TViewModel : BaseViewModel
        {
            if (viewModelsCache.ContainsKey(typeof(TViewModel)))
                viewModelsCache.Remove(typeof(TViewModel));
        }
    }
}
