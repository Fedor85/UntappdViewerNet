using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
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

        public static readonly DependencyProperty IsValidAccessTokenProperty = DependencyProperty.Register("IsValidAccessToken", typeof(bool?), typeof(AccessToken));

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
            Unloaded += AccessTokenUnloaded;

            TokenTextBox.SetBinding(SmartTextBox.TextProperty, new Binding { Path = new PropertyPath(TokenProperty), Mode = BindingMode.TwoWay, Source = this });
            TokenTextBox.SetBinding(SmartTextBox.IsShowPasswordModeProperty, new Binding { Path = new PropertyPath(IsShowPasswordProperty), Source = this });
            TokenTextBox.TextChanged = new DelegateCommand<string>(TokenTextBoxOnTextChanged);

            StatusButtonControl.SetBinding(StatusButton.ButtonTextProperty, new Binding { Path = new PropertyPath(ButtonTextProperty), Source = this });
            StatusButtonControl.SetBinding(StatusButton.IsValidStatusProperty, new Binding { Path = new PropertyPath(IsValidAccessTokenProperty), Source = this });
        }


        private void AccessTokenUnloaded(object sender, RoutedEventArgs e)
        {
            TokenTextBox.Clear();
        }

        private void TokenTextBoxOnTextChanged(string text)
        {
            IsValidAccessToken = null;
            IsShowPassword = IsShowPassword || String.IsNullOrEmpty(TokenTextBox.MainText);
            StatusButtonControl.IsEnabled = !String.IsNullOrEmpty(TokenTextBox.MainText);

        }

        private void AccessTokenButtonOnClick(object sender, RoutedEventArgs e)
        {
            AccessTokenCheck?.Execute(TokenTextBox.MainText);
        }
    }
}