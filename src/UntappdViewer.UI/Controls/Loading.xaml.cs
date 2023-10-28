using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FontAwesome.WPF;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        private static readonly DependencyProperty VisibilityImageAwesomeProperty = DependencyProperty.Register("VisibilityImageAwesome", typeof(Visibility), typeof(Loading));

        private static readonly DependencyProperty BackgroundStyleProperty = DependencyProperty.Register("BackgroundStyle", typeof(Style), typeof(Loading), new PropertyMetadata(Application.Current.FindResource("StyleLoadingControl")));

        public Visibility VisibilityImageAwesome
        {
            get { return (Visibility)GetValue(VisibilityImageAwesomeProperty); }
            set { SetValue(VisibilityImageAwesomeProperty, value); }
        }

        public Style BackgroundStyle
        {
            get { return (Style)GetValue(BackgroundStyleProperty); }
            set { SetValue(BackgroundStyleProperty, value); }
        }

        public Loading()
        {
            InitializeComponent();
            ImageAwesome.SetBinding(ImageAwesome.VisibilityProperty, new Binding { Path = new PropertyPath(VisibilityImageAwesomeProperty), Source = this });
            BackgroundControl.SetBinding(Grid.StyleProperty, new Binding { Path = new PropertyPath(BackgroundStyleProperty), Source = this });
        }
    }
}