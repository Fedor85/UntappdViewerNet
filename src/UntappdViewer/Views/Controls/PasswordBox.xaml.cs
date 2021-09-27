﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for PasswordBox.xaml
    /// </summary>
    public partial class PasswordBox : UserControl, INotifyPropertyChanged
    {
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public PasswordBox()
        {
            InitializeComponent();
        }

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
    

        private void TextPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = TextPasswordBox.Password;
            ImgShowHide.Visibility = TextPasswordBox.Password.Length > 0 ? Visibility.Visible : Visibility.Hidden;
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
            ImgShowHide.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Hide.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            TextVisiblePasswordBox.Visibility = Visibility.Visible;
            TextPasswordBox.Visibility = Visibility.Hidden;
            TextVisiblePasswordBox.Text = TextPasswordBox.Password;
        }

        private void HidePassword()
        {
            ImgShowHide.Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Show.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
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