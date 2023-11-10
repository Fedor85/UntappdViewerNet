using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for SmartTextBox.xaml
    /// </summary>
    public partial class SmartTextBox : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SmartTextBox), new PropertyMetadata(String.Empty));

        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(SmartTextBox), new PropertyMetadata(String.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        private string mainText;

        private bool isUpdateBinding;

        private bool passwordMode;
        
        private string mask;

        private int maxLength;

        public event Action<string> TextChanged;

        public double ImgSize
        {
            set
            {
                ImgShowHidePassword.Height = value;
                ImgShowHidePassword.Width = value;
            }
        }

        public bool PasswordMode
        {
            get { return passwordMode; }
            set { passwordMode = value; }
        }

        public string Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                maxLength = value;
                TextVisiblePasswordBox.MaxLength = value;
                TextPasswordBox.MaxLength = value;
            }
        }

        public SmartTextBox()
        {
            InitializeComponent();
            HintTextBox.SetBinding(TextBox.TextProperty, new Binding { Path = new PropertyPath(HintTextProperty), Source = this });
            
            Loaded += SmartTextBoxLoaded;
            Unloaded += SmartTextBoxUnloaded;
        }

        private void SmartTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            AttachedEvents();

            isUpdateBinding = GetIsUpdateBinding();
            CheckValidText();

            SetText(Text, TextSource.Binding);
        }

        private void SmartTextBoxUnloaded(object sender, RoutedEventArgs e)
        {
            DetachingEvents();
            mainText = String.Empty;
        }

        private void AttachedEvents()
        {
            HintTextBox.GotFocus += HintTextBoxGotFocus;

            TextPasswordBox.GotFocus += TextBoxGotFocus;
            TextPasswordBox.LostFocus += TextBoxLostFocus;
            TextPasswordBox.PasswordChanged += TextPasswordBoxPasswordChanged;

            TextVisiblePasswordBox.GotFocus += TextBoxGotFocus;
            TextVisiblePasswordBox.LostFocus += TextBoxLostFocus;
            TextVisiblePasswordBox.TextChanged += VisibleTextChanged;

            ImgShowHidePassword.MouseLeave += HidePasswordHandler;
            ImgShowHidePassword.PreviewMouseUp += HidePasswordHandler;
            ImgShowHidePassword.PreviewMouseDown += ShowPasswordHandler;
        }

        private void DetachingEvents()
        {
            HintTextBox.GotFocus -= HintTextBoxGotFocus;

            TextPasswordBox.GotFocus -= TextBoxGotFocus;
            TextPasswordBox.LostFocus -= TextBoxLostFocus;
            TextPasswordBox.PasswordChanged -= TextPasswordBoxPasswordChanged;

            TextVisiblePasswordBox.GotFocus -= TextBoxGotFocus;
            TextVisiblePasswordBox.LostFocus -= TextBoxLostFocus;
            TextVisiblePasswordBox.TextChanged -= VisibleTextChanged;

            ImgShowHidePassword.MouseLeave -= HidePasswordHandler;
            ImgShowHidePassword.PreviewMouseUp -= HidePasswordHandler;
            ImgShowHidePassword.PreviewMouseDown -= ShowPasswordHandler;
        }

        private bool GetIsUpdateBinding()
        {
            BindingExpression bindingExpression = GetBindingExpression(TextProperty);
            return bindingExpression != null && (bindingExpression.ParentBinding.Mode == BindingMode.TwoWay ||
                                                 bindingExpression.ParentBinding.Mode == BindingMode.OneWayToSource);
        }

        private void CheckValidText()
        {
            if(String.IsNullOrEmpty(Text))
                return;

            string currentText = Text;
            if (MaxLength > 0 && MaxLength < currentText.Length)
                currentText = currentText.Remove(MaxLength);

            if (!IsUpdateText(currentText))
                currentText = String.Empty;

            if (!Equals(Text, currentText))
                Text = currentText;
        }

        private bool IsUpdateText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Mask))
                return true;

            Regex regex = new Regex(Mask);
            MatchCollection matches = regex.Matches(text);
            return matches.Count == 1 && matches[0].Value.Equals(text);
        }

        public void Clear()
        {
           SetText(String.Empty, TextSource.Clear);
        }

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            string currentText = TextPasswordBox.Password;
            if (currentText.Equals(mainText))
                return;

            if (!IsUpdateText(currentText))
            {
                int currentIndex = GetPasswordBoxCurrentIndex();
                int delta = TextPasswordBox.Password.Length - mainText.Length;

                TextPasswordBox.Password = mainText;

                int currentCurrentIndex = currentIndex - delta;
                if(currentCurrentIndex >= 0)
                    TextPasswordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(TextPasswordBox, new object[] { currentCurrentIndex, 0 });
            }
            else
            {
                SetText(currentText, TextSource.PasswordBox);
            }
        }

        private int GetPasswordBoxCurrentIndex()
        {
            MethodInfo selectionMethod = TextPasswordBox.GetType().GetMethod("get_Selection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (selectionMethod == null)
                return 0;

            object textSelection = selectionMethod.Invoke(TextPasswordBox, null);
            if (textSelection == null)
                return 0;

            PropertyInfo propertyPosition = textSelection.GetType().GetProperty("PropertyPosition", BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyPosition == null)
                return 0;

            object passwordTextPosition = propertyPosition.GetValue(textSelection);
            if (passwordTextPosition == null)
                return 0;

            PropertyInfo offsetProperty = passwordTextPosition.GetType().GetProperty("Offset", BindingFlags.Instance | BindingFlags.NonPublic);
            if (offsetProperty == null)
                return 0;

            object offset = offsetProperty.GetValue(passwordTextPosition);
            return offset == null ? 0 : (int) offset;
        }

        private void VisibleTextChanged(object sender, TextChangedEventArgs e)
        {
            string currentText = TextVisiblePasswordBox.Text;
            if (currentText.Equals(mainText) || e.Changes.Count == 0)
                return;

            if (!IsUpdateText(currentText))
            {
                int currentIndex = e.Changes.ToList()[0].Offset;
                TextVisiblePasswordBox.Text = mainText;
                TextVisiblePasswordBox.CaretIndex = currentIndex;
            }
            else
            {
                SetText(currentText, TextSource.TextBox);
            }
        }

        private void SetText(string text, TextSource textSource)
        {

            mainText = text;

            DetachingEvents();

            if ((textSource == TextSource.TextBox || textSource == TextSource.Clear || textSource == TextSource.Binding) && passwordMode)
                TextPasswordBox.Password = mainText;

            if (textSource == TextSource.PasswordBox || textSource == TextSource.Clear || textSource == TextSource.Binding)

                TextVisiblePasswordBox.Text = mainText;

            if (textSource != TextSource.Binding && isUpdateBinding)
                Text = mainText;

            AttachedEvents();

            SetPasswordUIVisibility(mainText);
            SetHintTextVisibility(mainText);
            SetClearButtonVisibility(mainText);

            TextChanged?.Invoke(mainText);
        }

        private void SetHintTextVisibility(string text)
        {
            HintTextBox.Visibility = String.IsNullOrEmpty(text) && !String.IsNullOrEmpty(HintTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetClearButtonVisibility(string text)
        {
            ClearButton.Visibility = !String.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetPasswordUIVisibility(string text)
        {
            TextPasswordBox.Visibility = PasswordMode ? Visibility.Visible : Visibility.Collapsed;
            ImgShowHidePassword.Visibility = passwordMode && !String.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void HintTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordMode)
                TextPasswordBox.Focus();
            else
                TextVisiblePasswordBox.Focus();
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Collapsed;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            SetHintTextVisibility(mainText);
        }

        private void HidePasswordHandler(object sender, MouseEventArgs e)
        {
            ImgShowHidePassword.Source = ImageConverter.GetBitmapSource(Properties.Resources.Show);
            TextPasswordBox.Visibility = Visibility.Visible;
            TextPasswordBox.Focus();
        }

        private void ShowPasswordHandler(object sender, MouseEventArgs e)
        {
            ImgShowHidePassword.Source = ImageConverter.GetBitmapSource(Properties.Resources.Hide);
            TextPasswordBox.Visibility = Visibility.Hidden;
        }

        private void СlearClick(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        enum TextSource
        {
            TextBox,
            PasswordBox,
            Clear,
            Binding
        }
    }
}