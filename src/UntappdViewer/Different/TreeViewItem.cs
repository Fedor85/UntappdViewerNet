namespace UntappdViewer
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        private long id;

        public TreeViewItem(long id, string header)
        {
            this.id = id;
            Header = header;
        }
    }
}