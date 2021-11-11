using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            TreeViewItem treeViewItem = GetTreeViewItem(((SelectedTreeViewItemBehavior)dependencyObject).AssociatedObject, e.NewValue as TreeItemViewModel);
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

        private static TreeViewItem GetTreeViewItem(TreeView treeView, TreeItemViewModel newValue)
        {
            if (newValue == null)
                return null;

            TreeViewItem treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(newValue) as TreeViewItem;
            if (treeViewItem == null)
            {
                treeView.UpdateLayout();
                treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(newValue) as TreeViewItem;
            }
            if (treeViewItem == null)
            {
                int? index = GetIndex(treeView, newValue);
                if (index.HasValue)
                {
                    ItemsPresenter presenter = treeView.Template.FindName("ItemsPresenter", treeView) as ItemsPresenter;
                    VirtualizingStackPanel itemsHostPanel = VisualTreeHelper.GetChild(presenter, 0) as VirtualizingStackPanel;
                    itemsHostPanel.BringIndexIntoViewPublic(index.Value);

                    treeViewItem = treeView.ItemContainerGenerator.ContainerFromIndex(index.Value) as TreeViewItem;
                }
            }
            return treeViewItem;
        }

        private static int? GetIndex(TreeView treeView, TreeItemViewModel newValue)
        {
            for (int i = 0; i < treeView.Items.Count; i++)
            {
                TreeItemViewModel item = treeView.Items[i] as TreeItemViewModel;
                if (item.Id == newValue.Id)
                    return i;
            }
            return null;
        }
    }
}