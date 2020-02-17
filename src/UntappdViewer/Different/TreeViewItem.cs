namespace UntappdViewer
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        public long Id { get; }

        public TreeViewItem(long id, string header)
        {
            Id = id;
            Header = header;
        }
    }
}