using System.Collections;
using System.Linq;
using System.Windows.Controls;

namespace UntappdViewer.Views
{
    /// <summary>
    /// Interaction logic for GalleryProject.xaml
    /// </summary>
    public partial class GalleryProject : UserControl
    {
        public GalleryProject()
        {
            InitializeComponent();
        }

        private void ItemsSourceChanged(IEnumerable itemsSource)
        {
            int count = itemsSource?.Cast<object>().Count() ?? 0;
            ItemsCount.Content = $"{Properties.Resources.Count}: {count}";
        }
    }
}