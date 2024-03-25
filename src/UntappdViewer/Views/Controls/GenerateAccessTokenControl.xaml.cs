using System;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for GenerateAccessTokenControl.xaml
    /// </summary>
    public partial class GenerateAccessTokenControl : UserControl
    {

        public static readonly DependencyProperty GetCodeClickProperty = DependencyProperty.Register("GetCodeClick", typeof(ICommand), typeof(GenerateAccessTokenControl));

        public static readonly DependencyProperty GetAccessTokenClickProperty = DependencyProperty.Register("GetAccessTokenClick", typeof(ICommand), typeof(GenerateAccessTokenControl));

        public static readonly DependencyProperty IsValidAuthenticateUrlProperty = DependencyProperty.Register("IsValidAuthenticateUrl", typeof(bool?), typeof(GenerateAccessTokenControl), new PropertyMetadata(null, UpdateAuthenticateUrlStatus));

        public static readonly DependencyProperty AuthenticateUrlMessageTextProperty = DependencyProperty.Register("AuthenticateUrlMessageText", typeof(string), typeof(GenerateAccessTokenControl), new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsValidGenerateAccessTokenProperty = DependencyProperty.Register("IsValidGenerateAccessToken", typeof(bool?), typeof(GenerateAccessTokenControl), new PropertyMetadata(null, UpdateGenerateAccessTokenStatus));

        public static readonly DependencyProperty GenerateAccessTokenMessageTextProperty = DependencyProperty.Register("GenerateAccessTokenMessageText", typeof(string), typeof(GenerateAccessTokenControl), new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ICommand GetCodeClick
        {
            get { return (ICommand)GetValue(GetCodeClickProperty); }
            set { SetValue(GetCodeClickProperty, value); }
        }

        public ICommand GetAccessTokenClick
        {
            get { return (ICommand)GetValue(GetAccessTokenClickProperty); }
            set { SetValue(GetAccessTokenClickProperty, value); }
        }

        public bool? IsValidAuthenticateUrl
        {
            get { return (bool?)GetValue(IsValidAuthenticateUrlProperty); }
            set { SetValue(IsValidAuthenticateUrlProperty, value); }
        }

        public string AuthenticateUrlMessageText
        {
            get { return (string)GetValue(AuthenticateUrlMessageTextProperty); }
            set { SetValue(AuthenticateUrlMessageTextProperty, value); }
        }

        public bool? IsValidGenerateAccessToken
        {
            get { return (bool?)GetValue(IsValidGenerateAccessTokenProperty); }
            set { SetValue(IsValidGenerateAccessTokenProperty, value); }
        }

        public string GenerateAccessTokenMessageText
        {
            get { return (string)GetValue(GenerateAccessTokenMessageTextProperty); }
            set { SetValue(GenerateAccessTokenMessageTextProperty, value); }
        }

        public GenerateAccessTokenControl()
        {
            InitializeComponent();
            AuthenticateUrlMessage.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(AuthenticateUrlMessageTextProperty), Source = this, Mode = BindingMode.TwoWay});
            AccessTokenMessage.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(GenerateAccessTokenMessageTextProperty), Source = this, Mode = BindingMode.TwoWay });

            ClientIDTextBox.TextChanged = new DelegateCommand(InitialDataByClientIDTextChanged);
            RedirectUrlTextBox.TextChanged = new DelegateCommand(InitialDataByClientIDTextChanged);
            ClientSecretTextBox.TextChanged = new DelegateCommand(InitialDataByAccessTokenTextChanged);
            CodeTextBox.TextChanged = new DelegateCommand(InitialDataByAccessTokenTextChanged);
        }

        private void UpdateAuthenticateUrlStatus(bool? status)
        {
            if (status.HasValue)
            {
                AuthenticateUrlStatusImg.Visibility = Visibility.Visible;
                AuthenticateUrlStatusImg.Source = ImageConverter.GetBitmapSource(status.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
                SourceDataByCode.IsEnabled = !status.Value;
                SourceDataByAccessToken.IsEnabled = status.Value;
            }
            else
            {
                AuthenticateUrlStatusImg.Visibility = Visibility.Hidden;
                AuthenticateUrlMessage.Text = String.Empty;
                SourceDataByCode.IsEnabled = true;
                SourceDataByAccessToken.IsEnabled = false;
            }

            UpdateAccessTokenStatus(null);
            ClientSecretTextBox.Clear();
            CodeTextBox.Clear();
        }

        private void UpdateAccessTokenStatus(bool? status)
        {
            if (status.HasValue)
            {
                AccessTokenStatusImg.Visibility = Visibility.Visible;
                AccessTokenStatusImg.Source = ImageConverter.GetBitmapSource(status.Value ? Properties.Resources.green_checkmark : Properties.Resources.red_x);
            }
            else
            {
                AccessTokenMessage.Text = string.Empty;
                AccessTokenStatusImg.Visibility = Visibility.Hidden;
            }
        }

        private void InitialDataByClientIDTextChanged()
        {
            GetCodeButton.IsEnabled = !String.IsNullOrEmpty(ClientIDTextBox.MainText) && !String.IsNullOrEmpty(RedirectUrlTextBox.MainText);
        }

        private void InitialDataByAccessTokenTextChanged()
        {
            GetAccessTokenButton.IsEnabled = !String.IsNullOrEmpty(ClientSecretTextBox.MainText) && !String.IsNullOrEmpty(CodeTextBox.MainText);
        }

        private void GetCodeButtonClick(object sender, RoutedEventArgs e)
        {
            GetCodeClick?.Execute(new[] { ClientIDTextBox.MainText, RedirectUrlTextBox.MainText});
        }

        private void GetAccessTokenButtonClick(object sender, RoutedEventArgs e)
        {
            GetAccessTokenClick?.Execute(new[] { ClientIDTextBox.MainText, ClientSecretTextBox.MainText, RedirectUrlTextBox.MainText, CodeTextBox.MainText});
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ClientIDTextBox.Clear();
            RedirectUrlTextBox.Clear();
            UpdateAuthenticateUrlStatus(null);
        }

        private static void UpdateAuthenticateUrlStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GenerateAccessTokenControl generateAccessToken = dependencyObject as GenerateAccessTokenControl;
            generateAccessToken.UpdateAuthenticateUrlStatus(e.NewValue as bool?);

        }

        private static void UpdateGenerateAccessTokenStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GenerateAccessTokenControl generateAccessToken = dependencyObject as GenerateAccessTokenControl;
            generateAccessToken.UpdateAccessTokenStatus(e.NewValue as bool?);
        }
    }
}