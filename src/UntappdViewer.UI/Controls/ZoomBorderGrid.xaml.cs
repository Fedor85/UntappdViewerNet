using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ZoomBorderGrid.xaml
    /// </summary>
    public partial class ZoomBorderGrid : Border
    {
        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                bool b = IsInitialized;
                if (IsInitialized)
                {
                    if (value != null && value != Child)
                        ZoomBorderControl.Child = value;
                }
                else
                {
                    base.Child = value;
                }
            }
        }

        public ZoomBorderGrid()
        {
            InitializeComponent();
            ZoomBorderControl.IsInitializeEvent = false;

            MouseWheel += ZoomBorderGridMouseWheel;
            MouseLeftButtonDown += ZoomBorderGridMouseLeftButtonDown;
            MouseLeftButtonUp += ZoomBorderGridMouseLeftButtonUp;
            MouseMove += ZoomBorderGridMouseMove;
            PreviewMouseRightButtonDown += ZoomBorderGridPreviewMouseRightButtonDown;
        }

        private void ZoomBorderGridMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomBorderControl.ZoomChild(e);
        }

        private void ZoomBorderGridMouseMove(object sender, MouseEventArgs e)
        {
            ZoomBorderControl.MoveChild(e);
        }
        private void ZoomBorderGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ZoomBorderControl.ChildMouseLeftButtonDown(e);
        }

        private void ZoomBorderGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ZoomBorderControl.ChildMouseLeftButtonUp(e);
        }

        private void ZoomBorderGridPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ZoomBorderControl.ZoomChildReset();
        }
    }
}