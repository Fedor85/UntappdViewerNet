using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for StatusButton.xaml
    /// </summary>
    public partial class StatusButton : Button
    {
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(StatusButton));

        public static readonly DependencyProperty IsValidStatusProperty = DependencyProperty.Register("IsValidStatus", typeof(bool?), typeof(StatusButton), new PropertyMetadata(null, UpdateValidStatus));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public bool? IsValidStatus
        {
            get { return (bool?)GetValue(IsValidStatusProperty); }
            set { SetValue(IsValidStatusProperty, value); }
        }

        public StatusButton()
        {
            InitializeComponent();
            ButtonTextControl.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(ButtonTextProperty), Source = this });
        }

        private static void UpdateValidStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            StatusButton statusButton = dependencyObject as StatusButton;
            bool? status = e.NewValue as bool?;
            if (!status.HasValue)
            {
                statusButton.StatusImg.Visibility = Visibility.Hidden;
            }
            else
            {
                statusButton.StatusImg.Visibility = Visibility.Visible;
                statusButton.StatusImg.Source = ImageConverter.GetBitmapSource(status.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
            }
        }
    }
}