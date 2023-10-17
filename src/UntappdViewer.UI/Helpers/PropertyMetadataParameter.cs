using System.Windows;

namespace UntappdViewer.UI.Helpers
{
    public class PropertyMetadataParameter<T> : PropertyMetadata
    {
        public T Parameter { get; }

        public PropertyMetadataParameter(PropertyChangedCallback propertyChangedCallback, T parameter) : base(propertyChangedCallback)
        {
            Parameter = parameter;
        }
    }
}
