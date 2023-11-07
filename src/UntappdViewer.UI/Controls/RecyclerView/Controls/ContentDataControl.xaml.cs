using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UntappdViewer.UI.Controls.RecyclerView.Controls
{
    /// <summary>
    /// Interaction logic for ContentDataControl.xaml
    /// </summary>
    public partial class ContentDataControl : UserControl
    {
        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(ContentDataControl));


        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public ContentDataControl()
        {
            InitializeComponent();
            MainContent.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });
        }
    }
}