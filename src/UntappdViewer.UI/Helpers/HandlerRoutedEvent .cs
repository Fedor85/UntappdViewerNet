using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace UntappdViewer.UI.Helpers
{
    public class HandlerRoutedEvent(UIElement target)
    {
        private readonly MethodInfo genericHandlerMethod = typeof(HandlerRoutedEvent).GetMethod("Handler", BindingFlags.Instance | BindingFlags.NonPublic);
        
        public void Initialize(UIElement source, string eventNameContains = null)
        {
            IEnumerable<EventInfo> eventInfos = source.GetType().GetEvents();
            if (!String.IsNullOrEmpty(eventNameContains))
                eventInfos = eventInfos.Where(item => item.Name.ToLower().Contains(eventNameContains.ToLower()));

            foreach (EventInfo eventInfo in eventInfos)
            {
                Delegate handler = GetHandlerFor(eventInfo);
                eventInfo.AddEventHandler(source, handler);
            }
        }

        private void Handler<T>(object? sender, T args)
        {
            if (args is RoutedEventArgs routedEventArgs)
                target.RaiseEvent(routedEventArgs);
        }

        private Delegate GetHandlerFor(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            ParameterInfo[] parameterInfos = eventHandlerType.GetMethod("Invoke").GetParameters();
            if (parameterInfos.Length < 2)
                throw new ApplicationException("Couldn't get event args from eventInfo.");

            Type? eventArgsType = parameterInfos[1].ParameterType;
            if (eventArgsType is null)
                throw new ApplicationException("Couldn't get event args type from eventInfo.");

            MethodInfo handlerMethod = genericHandlerMethod.MakeGenericMethod(eventArgsType);
            if (handlerMethod is null)
                throw new ApplicationException("Couldn't get handlerMethod from genericHandlerMethod.");

            return Delegate.CreateDelegate(eventHandlerType, this, handlerMethod);
        }
    }
}