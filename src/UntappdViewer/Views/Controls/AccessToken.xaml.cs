using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UntappdViewer.UI.Controls;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for AccessToken.xaml
    /// </summary>
    public partial class AccessToken : UserControl
    {
        public static readonly DependencyProperty TokenProperty = DependencyProperty.Register("Token", typeof(string), typeof(AccessToken), new PropertyMetadata(String.Empty, BindingExtensions.UpdateSource));

        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(AccessToken), new PropertyMetadata(Properties.Resources.Check, BindingExtensions.UpdateSource));

        public static readonly DependencyProperty IsValidAccessTokenProperty = DependencyProperty.Register("IsValidAccessToken", typeof(bool?), typeof(AccessToken), new PropertyMetadata(null, UpdateCheckStatus));

        public static readonly DependencyProperty AccessTokenCheckProperty = DependencyProperty.Register("AccessTokenCheck", typeof(ICommand), typeof(AccessToken));

        public static readonly DependencyProperty IsShowPasswordProperty = DependencyProperty.Register("IsShowPassword", typeof(bool), typeof(AccessToken), new PropertyMetadata(true));

        public string Token
        {
            get { return (string)GetValue(TokenProperty); }
            set { SetValue(TokenProperty, value); }
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }


        public bool? IsValidAccessToken
        {
            get { return (bool?)GetValue(IsValidAccessTokenProperty); }
            set { SetValue(IsValidAccessTokenProperty, value); }
        }

        public ICommand AccessTokenCheck
        {
            get { return (ICommand)GetValue(AccessTokenCheckProperty); }
            set { SetValue(AccessTokenCheckProperty, value); }
        }

        public bool IsShowPassword
        {
            get { return (bool)GetValue(IsShowPasswordProperty); }
            set { SetValue(IsShowPasswordProperty, value); }
        }

        public AccessToken()
        {
            InitializeComponent();
            Loaded += AccessTokenLoaded;
            Unloaded += AccessTokenUnloaded;
            TokenTextBox.SetBinding(SmartTextBox.TextProperty, new Binding { Path = new PropertyPath(TokenProperty), Mode = BindingMode.TwoWay, Source = this });
            TokenTextBox.SetBinding(SmartTextBox.IsShowPasswordModeProperty, new Binding { Path = new PropertyPath(IsShowPasswordProperty), Source = this });
            ButtonTextControl.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(ButtonTextProperty), Source = this });
        }

        private void AccessTokenLoaded(object sender, RoutedEventArgs e)
        {
            TokenTextBox.TextChanged += TokenTextBoxOnTextChanged;
            UpdateCheckStatus();
        }

        private void AccessTokenUnloaded(object sender, RoutedEventArgs e)
        {
            TokenTextBox.TextChanged -= TokenTextBoxOnTextChanged;
            TokenTextBox.Clear();
        }

        private void TokenTextBoxOnTextChanged(string text)
        {
            if (IsValidAccessToken.HasValue)
                IsValidAccessToken = null;
            else
                UpdateCheckStatus();

            IsShowPassword = IsShowPassword || String.IsNullOrEmpty(TokenTextBox.MainText);
        }

        private void UpdateCheckStatus()
        {
            if (IsValidAccessToken.HasValue)
            {
                CheckStatusImg.Source = ImageConverter.GetBitmapSource(IsValidAccessToken.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
                CheckStatusImg.Visibility = Visibility.Visible;
                AccessTokenButton.IsEnabled = false;
            }
            else
            {
                CheckStatusImg.Visibility = Visibility.Hidden;
                AccessTokenButton.IsEnabled = !String.IsNullOrEmpty(TokenTextBox.MainText) && TokenTextBox.MainText.Length > 0;
            }
        }

        private void AccessTokenButtonOnClick(object sender, RoutedEventArgs e)
        {
            AccessTokenCheck?.Execute(TokenTextBox.MainText);
        }

        private static void UpdateCheckStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccessToken accessToken = dependencyObject as AccessToken;
            accessToken.UpdateCheckStatus();
        }
    }
}