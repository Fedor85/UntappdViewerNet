using System;
using System.Collections.Generic;
using System.Windows;
using UntappdViewer.Utils;

namespace UntappdViewer.UI.Controls.Maps.BingMap
{
    public static class BingMapService
    {
        public static readonly DependencyProperty RegisterMapProperty = DependencyProperty.RegisterAttached("RegisterMap", typeof(bool), typeof(BingMapService), new UIPropertyMetadata(false, AddMap));

        private static string credentialsProvider;

        private static List<BingMap> maps = new List<BingMap>();

        public static bool GetRegisterMap(DependencyObject obj)
        {
            return (bool)obj.GetValue(RegisterMapProperty);
        }

        public static void SetRegisterMap(DependencyObject obj, bool value)
        {
            obj.SetValue(RegisterMapProperty, value);
        }

        public static void InitializeCredentialsProvider(string credentialsProvider)
        {
            BingMapService.credentialsProvider = credentialsProvider;
        }

        public static void SetCredentialsProvider(BingMap map)
        {
            if (!String.IsNullOrEmpty(credentialsProvider) && maps.Contains(map))
                map.CredentialsProvider = credentialsProvider;
        }

        public static void UpdateCredentialsProvider(BingMap map)
        {
            if (StringHelper.AreEqual(map.CredentialsProvider, credentialsProvider))
                map.CredentialsProvider = String.Empty;

            SetCredentialsProvider(map);
        }
        private static void AddMap(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
                maps.Add(dependencyObject as BingMap);
        }
    }
}