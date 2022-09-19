using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UntappdViewer.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for SmartTextBox.xaml
    /// </summary>
    public partial class SmartTextBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("TextBinding", typeof(string), typeof(SmartTextBox), new FrameworkPropertyMetadata(null, SetText));

        private static object SetText(DependencyObject dependencyObject, object items)
        {
            ((SmartTextBox)dependencyObject).Text = (string)items;
            return items;
        }

        private bool passwordMode;

        private string text;

        private string mask;

        private string hintText;

        public event Action<string> TextChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public double ImgSize
        {
            set
            {
                ImgShowHide.Height = value;
                ImgShowHide.Width = value;
            }
        }

        public bool PasswordMode
        {
            get { return passwordMode; }
            set
            {
                passwordMode = value;
                TextPasswordBox.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (value)
                    ImgShowHide.Visibility = !String.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Hidden;
                else
                    ImgShowHide.Visibility = Visibility.Collapsed;          
            }
        }

        public string Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public string TextBinding
        {
            get { return (string)GetValue(DependencyProperty); }
            set
            {
                SetValue(DependencyProperty, value);
                OnPropertyChanged("TextBinding");
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (!TextPasswordBox.Password.Equals(value ?? String.Empty))
                    TextPasswordBox.Password = value;

                OnPropertyChanged("Text");
            }
        }

        public string HintText
        { 
            get { return hintText; }
            set
            {
                hintText = value;
                if (!String.IsNullOrEmpty(value))
                {
                    HintTextBox.Visibility = Visibility.Visible;
                    HintTextBox.Text = value;
                }
                OnPropertyChanged("HintText");
            }
        }

        public SmartTextBox()
        {
            InitializeComponent();
            PasswordMode = false;
            IsVisibleChanged += PasswordBoxIsVisibleChanged;

            HintTextBox.GotFocus += HintTextBoxGotFocus;
            TextPasswordBox.GotFocus += TextBoxGotFocus;
            TextVisiblePasswordBox.GotFocus += TextBoxGotFocus;

            TextPasswordBox.LostFocus += TextBoxLostFocus;
            TextVisiblePasswordBox.LostFocus += TextBoxLostFocus;          
        }

        private void HintTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Collapsed;
            if (TextPasswordBox.Visibility == Visibility.Visible)
                TextPasswordBox.Focus();
            else
                TextVisiblePasswordBox.Focus();
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Hidden;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(hintText) && String.IsNullOrEmpty(Text))
                HintTextBox.Visibility = Visibility.Visible;
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

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            if(TextPasswordBox.Password.Equals(Text))
                return;

            if (!String.IsNullOrEmpty(TextPasswordBox.Password) && !ChekMaskText(TextPasswordBox.Password))
            {

                int currentIndex = GetPasswordBoxCurrentIndex();
                int delta = TextPasswordBox.Password.Length - Text.Length;

                TextPasswordBox.Password = Text;

                int currentCurrentIndex = currentIndex - delta;
                if(currentCurrentIndex >= 0)
                    TextPasswordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(TextPasswordBox, new object[] { currentCurrentIndex, 0 });

                return;
            }

            TextVisiblePasswordBox.Text = TextPasswordBox.Password;
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
            if (TextVisiblePasswordBox.Text.Equals(Text))
                return;

            if (e.Changes.Count == 0)
                return;

            if (!String.IsNullOrEmpty(TextVisiblePasswordBox.Text) && !ChekMaskText(TextVisiblePasswordBox.Text))
            {
                int currentIndex = e.Changes.ToList()[0].Offset;
                TextVisiblePasswordBox.Text = Text;
                TextVisiblePasswordBox.CaretIndex = currentIndex;
                return;
            }

            Text = TextVisiblePasswordBox.Text;
            TextBinding = Text;

            if (PasswordMode)
                ImgShowHide.Visibility = !String.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Hidden;

            HintTextBox.Visibility = String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(HintTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            ClearButton.Visibility = !String.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private bool ChekMaskText(string text)
        {
            if (String.IsNullOrEmpty(Mask))
                return true;

            Regex regex = new Regex(Mask);
            MatchCollection matches = regex.Matches(text);
            return matches.Count == 1 && matches[0].Value.Equals(text);
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

            if (propertyName.Equals("Text") && TextChanged != null)
                TextChanged.Invoke(Text);
        }

        private void СlearClick(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}