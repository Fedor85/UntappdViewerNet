using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using UntappdViewer.UI.Controls;
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

        public static readonly DependencyProperty IsValidAuthenticateUrlProperty = DependencyProperty.Register("IsValidAuthenticateUrl", typeof(bool?), typeof(GenerateAccessTokenControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, UpdateAuthenticateUrlStatus));

        public static readonly DependencyProperty AuthenticateUrlMessageTextProperty = DependencyProperty.Register("AuthenticateUrlMessageText", typeof(string), typeof(GenerateAccessTokenControl), new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsValidGenerateAccessTokenProperty = DependencyProperty.Register("IsValidGenerateAccessToken", typeof(bool?), typeof(GenerateAccessTokenControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, UpdateGenerateAccessTokenStatus));

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

            AuthenticateUrlMessage.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(AuthenticateUrlMessageTextProperty), Source = this, Mode = BindingMode.TwoWay });
            AccessTokenMessage.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath(GenerateAccessTokenMessageTextProperty), Source = this, Mode = BindingMode.TwoWay });

            GetCodeButton.SetBinding(StatusButton.IsValidStatusProperty, new Binding { Path = new PropertyPath(IsValidAuthenticateUrlProperty), Source = this, Mode = BindingMode.TwoWay });
            GetAccessTokenButton.SetBinding(StatusButton.IsValidStatusProperty, new Binding { Path = new PropertyPath(IsValidGenerateAccessTokenProperty), Source = this, Mode = BindingMode.TwoWay });


            EnabledButtonController enabledGetCodeButton = new EnabledButtonController(GetCodeButton);
            enabledGetCodeButton.RegisterTextControl(ClientIDTextBox);
            enabledGetCodeButton.RegisterTextControl(RedirectUrlTextBox);

            EnabledButtonController enabledGetAccessTokenButton = new EnabledButtonController(GetAccessTokenButton);
            enabledGetAccessTokenButton.RegisterTextControl(ClientSecretTextBox);
            enabledGetAccessTokenButton.RegisterTextControl(CodeTextBox);
        }

        private void UpdateAuthenticateUrlStatus(bool? status)
        {
            if (status.HasValue)
            {
                SourceDataByCode.IsEnabled = !status.Value;
                SourceDataByAccessToken.IsEnabled = status.Value;
            }
            else
            {
                AuthenticateUrlMessage.Text = String.Empty;
                SourceDataByCode.IsEnabled = true;
                SourceDataByAccessToken.IsEnabled = false;
            }

            IsValidGenerateAccessToken = null;
            ClientSecretTextBox.Clear();
            CodeTextBox.Clear();
        }

        private void ClearAccessTokenMessage(bool? status)
        {
            if (!status.HasValue)
                AccessTokenMessage.Text = String.Empty;
        }

        private void GetCodeButtonClick(object sender, RoutedEventArgs e)
        {
            GetCodeClick?.Execute(new[] { ClientIDTextBox.Text, RedirectUrlTextBox.Text});
        }

        private void GetAccessTokenButtonClick(object sender, RoutedEventArgs e)
        {
            GetAccessTokenClick?.Execute(new[] { ClientIDTextBox.Text, ClientSecretTextBox.Text, RedirectUrlTextBox.Text, CodeTextBox.Text});
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ClientIDTextBox.Clear();
            RedirectUrlTextBox.Clear();
            IsValidAuthenticateUrl = null;
        }

        private static void UpdateAuthenticateUrlStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GenerateAccessTokenControl generateAccessToken = dependencyObject as GenerateAccessTokenControl;
            generateAccessToken.UpdateAuthenticateUrlStatus(e.NewValue as bool?);

        }

        private static void UpdateGenerateAccessTokenStatus(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GenerateAccessTokenControl generateAccessToken = dependencyObject as GenerateAccessTokenControl;
            generateAccessToken.ClearAccessTokenMessage(e.NewValue as bool?);
        }
    }
}