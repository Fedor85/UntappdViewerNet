﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace UntappdViewer.UI.Controls.Maps.GMapNet
{
    /// <summary>
    /// Interaction logic for LocationGMapNetControl.xaml
    /// </summary>
    public partial class LocationGMapNetControl : UserControl
    {
        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(LocationGMapNetControl));

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public LocationGMapNetControl()
        {
            InitializeComponent();
            MapControl.SetBinding(GMapNet.ItemDataTemplateProperty, new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });
        }

        private void MouseLeaved(object sender, MouseEventArgs e)
        {
            if (!MainControl.IsMouseOver && !Popup.IsMouseOver)
                Popup.IsOpen = false;
        }

        private void MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
        }
    }
}