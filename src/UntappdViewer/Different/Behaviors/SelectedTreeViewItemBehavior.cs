using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace UntappdViewer.Behaviors
{
    public class SelectedTreeViewItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(TreeViewItem),
                                                                                                        typeof(SelectedTreeViewItemBehavior),
                                                                                                        new PropertyMetadata(null, SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem item = e.NewValue as TreeViewItem;

            if (item != null)
            {
                item.Focus();
                item.BringIntoView();
                item.SetValue(System.Windows.Controls.TreeViewItem.IsSelectedProperty, true);
            }
        }

        public TreeViewItem SelectedItem
        {
            get
            {
                return (TreeViewItem)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += SelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.SelectedItemChanged -= SelectedItemChanged;
        }

        private void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = (TreeViewItem)e.NewValue;
        }
    }
}