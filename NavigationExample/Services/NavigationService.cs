using Microsoft.Extensions.DependencyInjection;
using NavigationExample.TemplateSelectors;
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
        private readonly Dictionary<PageDataTemplateSelector, Dictionary<Type, Page>> pagesCache;

        public NavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            modalWindowsCache = new Dictionary<object, Window>();
            nonModalWindowsCache = new Dictionary<object, Window>();
            pagesCache = new Dictionary<PageDataTemplateSelector, Dictionary<Type, Page>>();
        }

        private TViewModel GetViewModelInstance<TViewModel>(params object[] viewModelParameters)
            where TViewModel : BaseViewModel
        {
            return (TViewModel)GetViewModelInstance(typeof(TViewModel), viewModelParameters);
        }
        private BaseViewModel GetViewModelInstance(Type viewModelType, params object[] viewModelParameters)
        {
            return (BaseViewModel)ActivatorUtilities.CreateInstance(serviceProvider, viewModelType, viewModelParameters);
        }

        private TView GetViewInstance<TView>(object viewModel)
            where TView : class
        {
            return (TView)GetViewInstance(typeof(TView), viewModel);
        }
        private object GetViewInstance(Type viewType, object viewModel)
        {
            object view = Activator.CreateInstance(viewType);
            var prop = viewType.GetProperty("DataContext");
            prop?.SetValue(view, viewModel);
            return view;
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



        internal Page GetPage(PageDataTemplateSelector templateSelector, Type pageType, Type viewModelType, params object[] viewModelParameters)
        {
            if (!pagesCache.ContainsKey(templateSelector))
                pagesCache.Add(templateSelector, new Dictionary<Type, Page>());

            if (!pagesCache[templateSelector].ContainsKey(viewModelType))
                pagesCache[templateSelector].Add(viewModelType,
                                                 (Page)GetViewInstance(pageType, 
                                                                       GetViewModelInstance(viewModelType, 
                                                                                            viewModelParameters)));

            return pagesCache[templateSelector][viewModelType];
        }

        internal void DeleteTemplateSelector(PageDataTemplateSelector templateSelector)
        {
            if (pagesCache.ContainsKey(templateSelector))
                pagesCache.Remove(templateSelector);
        }
    }
}
