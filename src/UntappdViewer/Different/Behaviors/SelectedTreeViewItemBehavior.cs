using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace UntappdViewer.Behaviors
{
    public class SelectedTreeViewItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(TreeItemViewModel),
                                                                                                        typeof(SelectedTreeViewItemBehavior),
                                                                                                        new PropertyMetadata(null, SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem treeViewItem = ((SelectedTreeViewItemBehavior)dependencyObject).AssociatedObject.ItemContainerGenerator.ContainerFromItem(e.NewValue) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                treeViewItem.BringIntoView();
                treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
        }

        public TreeItemViewModel SelectedItem
        {
            get
            {
                return (TreeItemViewModel)GetValue(SelectedItemProperty);
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
            SelectedItem = (TreeItemViewModel)e.NewValue;
        }
    }
}