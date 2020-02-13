namespace UntappdViewer.Different
{
    public class TreeViewItem
    {
        private long id;

        private string displayName;

        public TreeViewItem(long id, string displayName)
        {
            this.id = id;
            this.displayName = displayName;
        }

        public override string ToString()
        {
            return displayName;
        }
    }
}