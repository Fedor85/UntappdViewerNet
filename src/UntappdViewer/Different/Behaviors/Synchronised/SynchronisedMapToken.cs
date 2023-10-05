using System.Windows;
using System.Windows.Input;
using UntappdViewer.UI.Controls;
using UntappdViewer.UI.Controls.GeoMap;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedMapToken : SynchronisedBaseToken<GeoMap>
    {
        public override void Register(GeoMap map)
        {
            base.Register(map);
            AttachedEvents(map);
        }

        public override void Unregister(GeoMap map)
        {
            base.Unregister(map);
            DetachingEvents(map);
        }

        private void AttachedEvents(GeoMap map)
        {
            map.ZoomMouseWheel += MapZoomMouseWheel;
            map.MouseLeftButtonDown += MapMouseLeftButtonDown;
            map.MouseLeftButtonUp += MapMouseLeftButtonUp;
            map.MouseRightButtonUp += MapMouseRightButtonUp;

            map.UndefinedGeoDataVisibility += MapUndefinedGeoDataVisibility;
        }

        private void DetachingEvents(GeoMap map)
        {
            map.ZoomMouseWheel -= MapZoomMouseWheel;
            map.MouseLeftButtonDown -= MapMouseLeftButtonDown;
            map.MouseLeftButtonUp -= MapMouseLeftButtonUp;
            map.MouseRightButtonUp -= MapMouseRightButtonUp;

            map.UndefinedGeoDataVisibility -= MapUndefinedGeoDataVisibility;
        }

        private void MapZoomMouseWheel(object sender, ZoomEventArgs e)
        {
            GeoMap sendingMap = GetObject(sender);
            if (sendingMap == null)
                return;

            foreach (GeoMap map in GetOtherItems(sendingMap))
                map.GeoMapZoom(e.Point, e.Delta);
        }

        private void MapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseLeftButtonDownEvent);
        }

        private void MapMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseLeftButtonUpEvent);
        }

        private void MapMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseRightButtonUpEvent);
        }

        private void MapButtonRaiseEvent(object sender, MouseButtonEventArgs e, RoutedEvent routedEvent)
        {
            GeoMap sendingMap = GetObject(sender);
            if (sendingMap == null)
                return;

            MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            mouseButtonEventArgs.RoutedEvent = routedEvent;
            foreach (GeoMap map in GetOtherItems(sendingMap))
            {
                DetachingEvents(map);
                map.RaiseEvent(mouseButtonEventArgs);
                AttachedEvents(map);
            }
        }

        private void MapUndefinedGeoDataVisibility(object sender, bool visible)
        {
            GeoMap sendingMap = GetObject(sender);
            if (sendingMap == null)
                return;

            foreach (GeoMap map in GetOtherItems(sendingMap))
            {
                DetachingEvents(map);
                map.UndefinedGeoDataSetVisibility(visible);
                AttachedEvents(map);
            }
        }
    }
}