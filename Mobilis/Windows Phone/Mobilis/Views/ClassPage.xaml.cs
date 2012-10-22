using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;

namespace Mobilis.Views
{
    public partial class ClassPage : PhoneApplicationPage
    {
        private ClassViewModel classViewModel;

        public ClassPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ClassPage_Loaded);
        }

        void ClassPage_Loaded(object sender, RoutedEventArgs e)
        {
            classViewModel = new ClassViewModel();
            ServiceLocator.Dispatcher = new DispatchAdapter();
            _classes.ItemsSource = classViewModel.classes;
            _classes.SelectionChanged -= _classes_SelectionChanged;
            _classes.SelectionChanged += new SelectionChangedEventHandler(_classes_SelectionChanged);
        }

        void _classes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_classes.SelectedIndex != -1)
            {
                if (classViewModel.existDiscussionsAtClass(_classes.SelectedIndex))
                {
                    NavigationService.Navigate(new Uri("/Views/DiscussionPage.xaml", UriKind.Relative));
                }

                else 
                {
                    classViewModel.requestDiscussions(() => 
                    {
                        ServiceLocator.Dispatcher.invoke(() => 
                        {
                            NavigationService.Navigate(new Uri("/Views/DiscussionPage.xaml", UriKind.Relative));
                        });
                    });                
                }
            }
        }
    }
}