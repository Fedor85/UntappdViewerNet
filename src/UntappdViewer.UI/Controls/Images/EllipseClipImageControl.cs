using System.Windows.Data;

namespace UntappdViewer.UI.Controls.Images
{
    public class EllipseClipImageControl: ImageEllipseClip
    {
        public EllipseClipImageControl()
        {
            SetBinding(ImageEllipseClip.ImageSourceProperty, new Binding("ImagePath"));
            SetBinding(ImageEllipseClip.ToolTipProperty, new Binding("ToolTip"));
        }
    }
}