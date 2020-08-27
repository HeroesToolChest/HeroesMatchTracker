using Avalonia.Controls;
using Avalonia.Controls.Templates;
using HeroesMatchTracker.ViewModels;
using System;

namespace HeroesMatchTracker
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            string name = data.GetType()!.FullName!.Replace("ViewModel", "View", StringComparison.OrdinalIgnoreCase);
            Type? type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}