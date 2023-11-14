using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace UntappdViewer.UI.Helpers
{
    public static class BindingExtensions
    {
        private delegate object UpdateSourceDelegate(BindingExpression expression, object value);

        private static readonly UpdateSourceDelegate updateSourceDelegate;

        static BindingExtensions()
        {
            MethodInfo methodInfo = typeof(BindingExpression).GetMethod("UpdateSource", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(object) }, null);
            updateSourceDelegate = (UpdateSourceDelegate)Delegate.CreateDelegate(typeof(UpdateSourceDelegate), methodInfo);
        }

        private static void UpdateSource(this BindingExpression expression, object value)
        {
             updateSourceDelegate(expression, value);
        }

        public static void UpdateSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateSource(dependencyObject as FrameworkElement, e.Property, e.NewValue);
        }

        public static void UpdateSource(this FrameworkElement frameworkElement, DependencyProperty dependency, object value)
        {
            BindingExpression bindingExpression = frameworkElement.GetBindingExpression(dependency);
            bindingExpression?.UpdateSource(value);
        }
    }
}
