﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UntappdViewer.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for PasswordBox.xaml
    /// </summary>
    public partial class PasswordBox : UserControl
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(null, SetPassword));

        private static object SetPassword(DependencyObject dependencyObject, object text)
        {
            if (text != null)
                ((PasswordBox)dependencyObject).Password = (string)text;

            return text;
        }

        private bool passwordMode;

        private string password;

        public event Action<string> PasswordChanged;

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
                    ImgShowHide.Visibility = !String.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Hidden;
                else
                    ImgShowHide.Visibility = Visibility.Collapsed;          
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                if (PasswordChanged != null)
                    PasswordChanged.Invoke(value);
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

        public PasswordBox()
        {
            InitializeComponent();
            PasswordMode = true;

            IsVisibleChanged += PasswordBoxIsVisibleChanged;
            HintTextBox.GotKeyboardFocus += HintTextBoxGotKeyboardFocus;
            TextPasswordBox.GotKeyboardFocus += TextPasswordBoxKeyboardFocus;
            TextVisiblePasswordBox.GotKeyboardFocus += TextVisiblePasswordBoxGotKeyboardFocus;
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
            if (TextPasswordBox.Visibility == Visibility.Visible)
                TextPasswordBox.Focus();
            else
                TextVisiblePasswordBox.Focus();
        }

        private void TextPasswordBoxKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Hidden;
        }

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            TextVisiblePasswordBox.Text = TextPasswordBox.Password;
        }

        private void VisibleTextChanged(object sender, TextChangedEventArgs e)
        {
            Password = TextVisiblePasswordBox.Text;
            if (PasswordMode)
                ImgShowHide.Visibility = !String.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Hidden;

            HintTextBox.Visibility = String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(HintTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextVisiblePasswordBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HintTextBox.Visibility = Visibility.Hidden;
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
    }
}
