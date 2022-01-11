// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using ReactiveUI;

using RoutedNavigation.ViewModels;

namespace RoutedNavigation.Views
{

    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}
