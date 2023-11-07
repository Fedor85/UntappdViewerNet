using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.Images
{
    public class SimpleImageControl: Image
    {
        public SimpleImageControl()
        {
            Stretch = Stretch.Uniform;
            SetBinding(Image.SourceProperty, new Binding("ImagePath"));
            SetBinding(Image.ToolTipProperty, new Binding("ToolTip"));
        }
    }
}