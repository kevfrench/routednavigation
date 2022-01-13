// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ModernWpf.Controls;
using ReactiveUI;

using RoutedNavigation.ViewModels;

namespace RoutedNavigation
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.Frame.Router)
                    .DisposeWith(disposables);

                // Bind the navigation items
                this.OneWayBind(ViewModel, vm => vm.MenuItems, v => v.Navigation.MenuItemsSource)
                    .DisposeWith(disposables);

                // Ensure the UI matches the actual navigation
                this.Bind(ViewModel, vm => vm.SelectedItem, v => v.Navigation.SelectedItem)
                    .DisposeWith(disposables);

                // Bind the back button, have to do this funky since it's an attached property
                _ = this.WhenAnyValue(v => v.ViewModel.GoBack)
                    .Subscribe(command => TitleBar.SetBackButtonCommand(this, command))
                    .DisposeWith(disposables);

            });
        }
    }
}
