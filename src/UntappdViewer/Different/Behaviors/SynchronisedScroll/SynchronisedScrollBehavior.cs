using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedScrollBehavior : Behavior<ScrollViewer>
    {

        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("RegisterToken", typeof(SynchronisedScrollToken), typeof(SynchronisedScrollBehavior), null);


        public SynchronisedScrollToken RegisterToken
        {
            get
            {
                return (SynchronisedScrollToken)GetValue(DependencyProperty);
            }
            set
            {
                SetValue(DependencyProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            RegisterToken?.Register(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            RegisterToken?.Unregister(AssociatedObject);
        }
    }
}