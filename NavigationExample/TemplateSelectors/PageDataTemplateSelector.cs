using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace NavigationExample.TemplateSelectors
{
    [ContentProperty(nameof(Pages))]
    public class PageDataTemplateSelector : DataTemplateSelector, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<Type, Page> pagesCache;

        public DataTemplate PageTemplate { get; }

        private Page currentPage;
        public Page CurrentPage => currentPage;

        public Dictionary<Type, Type> Pages { get; }

        public PageDataTemplateSelector()
        {
            pagesCache = new Dictionary<Type, Page>();
            Pages = new Dictionary<Type, Type>();
            PageTemplate = CreatePageDataTemplate();
        }


        private DataTemplate CreatePageDataTemplate()
        {
            var pageTemplate = new DataTemplate();

            //set up the frame
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(Frame));
            spFactory.SetValue(Frame.NavigationUIVisibilityProperty, NavigationUIVisibility.Hidden);
            spFactory.SetBinding(Frame.ContentProperty, new Binding(nameof(CurrentPage))
            {
                Source = this
            });

            //set the visual tree of the data template
            pageTemplate.VisualTree = spFactory;

            return pageTemplate;
        }

        public override DataTemplate SelectTemplate(object viewModel, DependencyObject container)
        {
            var vmType = viewModel?.GetType();
            if (viewModel == null ||
                Pages == null ||
                !Pages.ContainsKey(vmType))
            {
                currentPage = null;
            }
            else
            {
                if (pagesCache.ContainsKey(vmType))
                    currentPage = pagesCache[vmType];
                else
                {
                    currentPage = (Page)Activator.CreateInstance(Pages[viewModel.GetType()]);
                    currentPage.DataContext = viewModel;
                    pagesCache[vmType] = currentPage;
                }
                
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
            
            return PageTemplate;
        }
    }
}
