using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ListViewNoSelect.xaml
    /// </summary>
    public partial class ListViewNoSelect : UserControl
    {
        private static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(ListViewNoSelect), new PropertyMetadata(Initialize));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ListViewNoSelect), new PropertyMetadata(Initialize));

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(ListViewNoSelect));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }


        public ListViewNoSelect()
        {
            InitializeComponent();

            CaptionControl.SetBinding(Label.ContentProperty, new Binding { Path = new PropertyPath(CaptionProperty), Source = this });
            ListView.SetBinding(ListView.ItemsSourceProperty, new Binding { Path = new PropertyPath(ItemsSourceProperty), Source = this });
            ListView.SetBinding(ListView.ItemTemplateProperty , new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });
        }

        private void Initialize()
        {
            CaptionControl.Visibility = ItemsSource != null && ItemsSource.Count > 0 && !String.IsNullOrEmpty(Caption)? Visibility.Visible : Visibility.Collapsed;
        }

        private static void Initialize(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ListViewNoSelect listView = dependencyObject as ListViewNoSelect;
            listView?.Initialize();
        }
    }
}
