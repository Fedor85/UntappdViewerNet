using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UntappdViewer.UI.Controls
{
    public class TextBlockJoin: TextBlock
    {
        private static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register("Separator", typeof(string), typeof(TextBlockJoin), new PropertyMetadata(", "));

        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        public TextBlockJoin():base()
        {
            DataContextChanged += TextDataContextChanged;
        }

        private void TextDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Inlines.Clear();
            IList collection = e.NewValue as IList;
            if (collection == null)
                return;

            int count = collection.Count;
            for (int i = 0; i < count; i++)
            {
                Inlines.Add(collection[i].ToString());
                if (i == count - 1)
                    break;

                Run separator = new Run(Separator);
                separator.FontWeight = FontWeights.Bold;
                Inlines.Add(separator);
            }
        }
    }
}