using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UntappdViewer.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for PasswordBox.xaml
    /// </summary>
    public partial class PasswordBox : UserControl, INotifyPropertyChanged
    {
        private string password;

        public double ImgSize
        {
            set
            {
                ImgShowHide.Height = value;
                ImgShowHide.Width = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string HintText
        { 
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    HintTextBox.Visibility = Visibility.Visible;
                    HintTextBox.Text = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PasswordBox()
        {
            InitializeComponent();
            IsVisibleChanged += PasswordBoxIsVisibleChanged;
            HintTextBox.GotKeyboardFocus += HintTextBoxGotKeyboardFocus;
        }


        public void Clear()
        {
            TextPasswordBox.Clear();
            TextVisiblePasswordBox.Clear();
        }

        private void PasswordBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && (bool)e.NewValue)
                HintTextBox.Visibility = String.IsNullOrEmpty(TextPasswordBox.Password) && !String.IsNullOrEmpty(HintTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void HintTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Collapsed;
            TextPasswordBox.Focus();
        }

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = TextPasswordBox.Password;
            ImgShowHide.Visibility = !String.IsNullOrEmpty(TextPasswordBox.Password) ? Visibility.Visible : Visibility.Hidden;
            HintTextBox.Visibility = String.IsNullOrEmpty(TextPasswordBox.Password) && !String.IsNullOrEmpty(HintTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ImgShowHideMouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHidePreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHidePreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void ShowPassword()
        {
            ImgShowHide.Source = ImageConverter.GetBitmapSource(Properties.Resources.Hide);
            TextVisiblePasswordBox.Visibility = Visibility.Visible;
            TextPasswordBox.Visibility = Visibility.Hidden;
            TextVisiblePasswordBox.Text = TextPasswordBox.Password;
        }

        private void HidePassword()
        {
            ImgShowHide.Source = ImageConverter.GetBitmapSource(Properties.Resources.Show);
            TextVisiblePasswordBox.Visibility = Visibility.Hidden;
            TextPasswordBox.Visibility = Visibility.Visible;
            TextPasswordBox.Focus();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
