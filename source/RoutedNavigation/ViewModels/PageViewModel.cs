// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using System.Reactive.Disposables;

using ReactiveUI;
using Splat;

namespace RoutedNavigation.ViewModels
{

    public class PageViewModel : ReactiveObject,
        IRoutableViewModel, IActivatableViewModel, IEnableLogger
    {
        public PageViewModel()
        {
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                OnActivated(disposables);
                Disposable.Create(OnDeactivated)
                    .DisposeWith(disposables);
            });
        }

        public string UrlPathSegment { get; set; }

        public IScreen HostScreen { get; set; }

        public ViewModelActivator Activator { get; }
            = new ViewModelActivator();

        protected virtual void OnActivated(CompositeDisposable disposables)
        {
            this.Log().Debug($"'{GetType()}' activated.");
        }

        protected virtual void OnDeactivated()
        {
            this.Log().Debug($"'{GetType()}' deactivated.");
        }
    }
}
