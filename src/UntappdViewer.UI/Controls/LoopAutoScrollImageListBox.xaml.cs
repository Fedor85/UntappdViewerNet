using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for LoopAutoScrollImageListBox.xaml
    /// </summary>
    public partial class LoopAutoScrollImageListBox : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(LoopAutoScrollImageListBox), new PropertyMetadata(SetItemsSource));

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(LoopAutoScrollImageListBox));

        private Timer timer;

        private int speed;

        private ObservableCollection<object> collection;

        private int collectionCount;

        public int MinItemsCount { get; set; }


        public int Speed
        {
            set
            {
                speed = value;
                if (value > 0)
                    timer.Interval = speed;
            }
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

        public LoopAutoScrollImageListBox()
        {
            InitializeComponent();
            MainListBox.SetBinding(ListBox.ItemTemplateProperty, new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });

            MinItemsCount = 2;
            ItemsSource = new List<object>();

            Loaded += WindowLoaded;

            timer = new Timer();
            timer.Elapsed += TimerElapsed;
        }

        private void Run()
        {
            if (ItemsSource == null || ItemsSource.Count <= MinItemsCount)
                return;

            collection = new ObservableCollection<object>(ItemsSource.Cast<object>());
            collectionCount = collection.Count - 1;

            MainListBox.ItemsSource = collection;
            timer.Start();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UserControl thisControl = e.OriginalSource as UserControl;
            Window window = UIHelper.GetWindow(thisControl);
            if (window == null)
                return;

            window.Closing += WindowClosing;
            window.Unloaded += WindowUnloaded;
        }

        private void WindowUnloaded(object sender, RoutedEventArgs e)
        {  
            ItemsSource?.Clear();
            collection?.Clear();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            timer.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(()=> collection.Move(0, collectionCount));
        }

        private static void SetItemsSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != null)
                ((LoopAutoScrollImageListBox)dependencyObject).Run();
        }
    }
}