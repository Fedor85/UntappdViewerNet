using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace UntappdViewer.Behaviors
{
    public class ParentScrollingBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty ParentDependencyProperty = DependencyProperty.Register("ParentScrollViewer", typeof(ParentScrollingToken), typeof(ParentScrollingBehavior));

        public static readonly DependencyProperty ChildDependencyProperty =  DependencyProperty.Register("ChildScrollViewer", typeof(ParentScrollingToken), typeof(ParentScrollingBehavior));

        public ParentScrollingToken ParentScrollViewer
        {
            get { return (ParentScrollingToken) GetValue(ParentDependencyProperty); }
            set { SetValue(ParentDependencyProperty, value); }
        }

        public ParentScrollingToken ChildScrollViewer
        {
            get { return (ParentScrollingToken) GetValue(ChildDependencyProperty); }
            set { SetValue(ChildDependencyProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            ParentScrollViewer?.SetParentScrollViewer(AssociatedObject);
            ChildScrollViewer?.AddChildScrollViewer(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ParentScrollViewer?.Clear();
            ChildScrollViewer?.Clear();
        }
    }
}
