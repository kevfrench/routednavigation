// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using System.Windows;

using ReactiveUI;
using Splat;

using RoutedNavigation.ViewModels;
using RoutedNavigation.Views;

namespace RoutedNavigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            RegisterPage<HomeViewModel, HomeView>();
            RegisterPage<CollectionsViewModel, CollectionsView>();
            RegisterPage<NotesViewModel, NotesView>();
            RegisterPage<MailViewModel, MailView>();
        }

        private void RegisterPage<TViewModel, TView>()
            where TViewModel : class, IRoutableViewModel, new()
            where TView : IViewFor<TViewModel>, new()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new TViewModel());
            Locator.CurrentMutable.Register<IViewFor<TViewModel>>(() => new TView());
        }
    }
}
