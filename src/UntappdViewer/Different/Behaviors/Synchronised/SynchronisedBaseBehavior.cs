using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace UntappdViewer.Behaviors
{
    public class SynchronisedBaseBehavior<TControl, TToken > : Behavior<TControl> where TControl : DependencyObject,
                                                                             new() where TToken : SynchronisedBaseToken<TControl>
    {

        public static readonly DependencyProperty DependencyProperty = 
            DependencyProperty.Register("RegisterToken", typeof(SynchronisedBaseToken<TControl>), typeof(SynchronisedBaseBehavior<TControl, TToken>), null);

        public TToken RegisterToken
        {
            get
            {
                return (TToken)GetValue(DependencyProperty);
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