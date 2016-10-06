using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HeroesParserData
{
    //http://nerobrain.blogspot.com/2012/01/execute-command-on-combobox-selection.html
    public class SelectionChangedBehaviour
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand),
       typeof(SelectionChangedBehaviour), new PropertyMetadata(PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            Selector selector = (Selector)depObj;
            if (selector != null)
            {
                selector.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged);
            }
        }

        public static ICommand GetCommand(UIElement element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommand(UIElement element, ICommand command)
        {
            element.SetValue(CommandProperty, command);
        }

        private static void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = (Selector)sender;
            if (selector != null)
            {
                ICommand command = selector.GetValue(CommandProperty) as ICommand;
                if (command != null)
                {
                    command.Execute(selector.SelectedItem);
                }
            }
        }
    }
}
