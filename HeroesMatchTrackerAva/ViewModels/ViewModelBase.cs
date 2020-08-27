using ReactiveUI;

namespace HeroesMatchTracker.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}
