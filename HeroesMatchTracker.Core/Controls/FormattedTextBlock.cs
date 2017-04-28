using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace HeroesMatchTracker.Core.Controls
{
    public static class FormattedTextBlock
    {
        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached(
                "FormattedText",
                typeof(Inline),
                typeof(FormattedTextBlock),
                new PropertyMetadata(null, OnFormattedTextChanged));

        public static Inline GetFormattedText(DependencyObject obj)
        {
            return (Inline)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, Inline value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        private static void OnFormattedTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = o as TextBlock;
            if (textBlock == null) return;

            var inline = (Inline)e.NewValue;
            if (inline == null)
            {
                textBlock.Inlines.Clear();
            }
            else
            {
                textBlock.Inlines.Add(inline);
            }
        }
    }
}
