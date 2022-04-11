using System.Windows;
using Microsoft.Xaml.Behaviors;
using UntappdViewer.Views;

namespace UntappdViewer.Behaviors
{
    public class AccessTokenBehavior : Behavior<WebDownloadProject>
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("AccessToken", typeof(bool?), typeof(AccessTokenBehavior), new PropertyMetadata(null, AccessTokenPropertyChangedCallback));

        private static void AccessTokenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccessTokenBehavior accessTokenBehavior = dependencyObject as AccessTokenBehavior;
            accessTokenBehavior.AssociatedObject.SetAccessToken(accessTokenBehavior.AccessToken);
        }

        public bool? AccessToken
        {
            get
            {
                return (bool?)GetValue(DependencyProperty);
            }
            set
            {
                SetValue(DependencyProperty, value);
            }
        }
    }
}