using System;
using System.Windows;
using System.Windows.Controls;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace UntappdViewer.UI.Controls.Maps.GMapNet
{
    public class MapMarker: GMapMarker
    {
        private ContentPresenter contentPresenter;

        public string ToolTip { get; set; }

        public ContentPresenter ContentPresenter
        {
            get { return contentPresenter; }
            set
            {
                contentPresenter = value;

                if (!String.IsNullOrEmpty(ToolTip))
                    contentPresenter.ToolTip = ToolTip;

                contentPresenter.Loaded += ContentPresenterLoaded;
            }
        }

        public MapMarker(PointLatLng pos) : base(pos)
        {
        }

        private void ContentPresenterLoaded(object sender, RoutedEventArgs e)
        {
            SetOffset();
            contentPresenter.Loaded -= ContentPresenterLoaded;
        }

        public void SetOffset()
        {
            Offset = new Point(-ContentPresenter.ActualWidth / 2, -ContentPresenter.ActualHeight / 2);
        }

        public bool IsContentPresenterLoaded()
        {
            return ContentPresenter != null && ContentPresenter.IsLoaded;
        }
    }
}