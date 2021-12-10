using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using UntappdViewer.Different.Helpers;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for LoopAutoScrollImageListBox.xaml
    /// </summary>
    public partial class LoopAutoScrollImageListBox : UserControl
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(LoopAutoScrollImageListBox), new FrameworkPropertyMetadata(null, SetItemsSource));

        private static object SetItemsSource(DependencyObject dependencyObject, object items)
        {
            if (items != null)
                ((LoopAutoScrollImageListBox)dependencyObject).ItemsSource = (IEnumerable)items;

            return items;
        }

        private ObservableCollection<string> items;

        private Timer timer;

        private int minItemsCount;

        private int speed;

        public int MinItemsCount
        {
            get { return minItemsCount; }
            set
            {
                minItemsCount = value;
                RunTimer();
            }
        }

        public int Speed
        {
            set
            {
                speed = value;
                if (value > 0)
                    timer.Interval = speed;
            }
        }

        public IEnumerable ItemsSource
        {
            set
            {
                ObservableCollection<string> itemsSource = value as ObservableCollection<string>;
                if (itemsSource.Count >= MinItemsCount)
                {
                    items = itemsSource;
                    ListBox.ItemsSource = items;
                }
                else
                {
                    items.Clear();
                }
                RunTimer();
            }
        }

        public LoopAutoScrollImageListBox()
        {
            InitializeComponent();
            Loaded += WindowLoaded;

            timer = new Timer();
            timer.Elapsed += TimerElapsed;

            items = new ObservableCollection<string>();
            MinItemsCount = 1;
        }

        private void RunTimer()
        {
            if (items.Count >= Math.Max(MinItemsCount, 2))
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
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => items.Move(0, items.Count - 1));
        }
    }
}