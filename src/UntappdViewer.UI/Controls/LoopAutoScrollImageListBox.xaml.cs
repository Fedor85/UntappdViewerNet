using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for LoopAutoScrollImageListBox.xaml
    /// </summary>
    public partial class LoopAutoScrollImageListBox : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(LoopAutoScrollImageListBox), new PropertyMetadata(null, SetItemsSource));

        private Timer timer;

        private int speed;

        private ObservableCollection<string> collection;

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

        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public LoopAutoScrollImageListBox()
        {
            InitializeComponent();
            MinItemsCount = 2;
            ItemsSource = new List<string>();

            Loaded += WindowLoaded;

            timer = new Timer();
            timer.Elapsed += TimerElapsed;
        }

        private void Run()
        {
            if (ItemsSource == null || ItemsSource.Count <= MinItemsCount)
                return;

            collection = new ObservableCollection<string>(ItemsSource);
            collectionCount = collection.Count - 1;

            ListBox.ItemsSource = collection;
            timer.Start();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UserControl thisControl = e.OriginalSource as UserControl;
            Window window = UIHelper.GetWindow(thisControl);
            if (window != null)
                window.Closing += WindowClosing;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            timer.Stop();
            ItemsSource.Clear();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(()=> collection.Move(0, collectionCount));
        }

        private static void SetItemsSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((LoopAutoScrollImageListBox)dependencyObject).Run();
        }
    }
}