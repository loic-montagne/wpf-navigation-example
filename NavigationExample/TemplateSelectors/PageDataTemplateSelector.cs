using Microsoft.Extensions.DependencyInjection;
using NavigationExample.Services;
using NavigationExample.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace NavigationExample.TemplateSelectors
{
    [ContentProperty(nameof(Pages))]
    public class PageDataTemplateSelector : DataTemplateSelector, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private NavigationService navigationService;

        public DataTemplate PageTemplate { get; }

        private Page currentPage;
        public Page CurrentPage => currentPage;

        public Dictionary<Type, Type> Pages { get; set; }

        public PageDataTemplateSelector()
        {
            navigationService = ((App)Application.Current).ServiceProvider.GetService<NavigationService>();
            Pages = new Dictionary<Type, Type>();
            PageTemplate = CreatePageDataTemplate();
        }
        ~PageDataTemplateSelector()
        {
            navigationService.DeleteTemplateSelector(this);
        }


        private DataTemplate CreatePageDataTemplate()
        {
            var pageTemplate = new DataTemplate();

            //set up the frame
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(Frame));
            spFactory.SetValue(Frame.NavigationUIVisibilityProperty, System.Windows.Navigation.NavigationUIVisibility.Hidden);
            spFactory.SetBinding(Frame.ContentProperty, new Binding(nameof(CurrentPage))
            {
                Source = this
            });

            //set the visual tree of the data template
            pageTemplate.VisualTree = spFactory;

            return pageTemplate;
        }

        public override DataTemplate SelectTemplate(object viewModelType, DependencyObject container)
        {
            if (viewModelType == null ||
                !(viewModelType is Type vmType) ||
                Pages == null ||
                !Pages.ContainsKey(vmType))
                currentPage = null;
            else
                currentPage = navigationService.GetPage(this, Pages[vmType], vmType);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
            
            return PageTemplate;
        }
    }
}
