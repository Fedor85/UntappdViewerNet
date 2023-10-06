using System;
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

            map.MapPathMouseEnter += MapPathMouseEnter;
            map.MapPathMouseLeave += MapPathMouseLeave;
            map.MapPathMouseMove += MapPathMouseMove;

            map.UndefinedGeoDataVisibility += MapUndefinedGeoDataVisibility;
        }

        private void DetachingEvents(GeoMap map)
        {
            map.ZoomMouseWheel -= MapZoomMouseWheel;
            map.MouseLeftButtonDown -= MapMouseLeftButtonDown;
            map.MouseLeftButtonUp -= MapMouseLeftButtonUp;
            map.MouseRightButtonUp -= MapMouseRightButtonUp;

            map.MapPathMouseEnter -= MapPathMouseEnter;
            map.MapPathMouseLeave -= MapPathMouseLeave;
            map.MapPathMouseMove -= MapPathMouseMove;

            map.UndefinedGeoDataVisibility -= MapUndefinedGeoDataVisibility;
        }

        private void MapZoomMouseWheel(object sender, ZoomEventArgs e)
        {
            UpdateOtheMaps(sender, map => { map.GeoMapZoom(e.Point, e.Delta); });
        }

        private void MapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseLeftButtonDownEvent);
        }

        private void MapMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseLeftButtonUpEvent);
        }

        private void MapMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapButtonRaiseEvent(sender, e, UIElement.MouseRightButtonUpEvent);
        }

        private void MapButtonRaiseEvent(object sender, MouseButtonEventArgs e, RoutedEvent routedEvent)
        {
            MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            mouseButtonEventArgs.RoutedEvent = routedEvent;
            UpdateOtheMaps(sender, map => { map.RaiseEvent(mouseButtonEventArgs); });
        }

        private void MapPathMouseEnter(object sender, string pathName)
        {
            UpdateOtheMaps(sender, map => { map.PathEnter(pathName); });
        }

        private void MapPathMouseLeave(object sender, string pathName)
        {
            UpdateOtheMaps(sender, map => { map.PathLeave(pathName); });
        }

        private void MapPathMouseMove(object sender, Point point)
        {
            UpdateOtheMaps(sender, map => { map.PathMouseMove(point); });
        }

        private void MapUndefinedGeoDataVisibility(object sender, bool visible)
        {
            UpdateOtheMaps(sender, map => { map.UndefinedGeoDataSetVisibility(visible); });   
        }

        private void UpdateOtheMaps(object sender, Action<GeoMap> funcUpdate)
        {
            GeoMap sendingMap = GetObject(sender);
            if (sendingMap == null)
                return;

            foreach (GeoMap map in GetOtherItems(sendingMap))
            {
                DetachingEvents(map);
                funcUpdate(map);
                AttachedEvents(map);
            }
        }
    }
}