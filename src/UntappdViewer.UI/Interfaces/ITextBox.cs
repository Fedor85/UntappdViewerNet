using System.Windows.Input;

namespace UntappdViewer.UI.Interfaces
{
    public interface ITextBox
    {
        public string Text { get;}

        public ICommand TextChanged { get; set; }
    }
}
