using System;
using System.ComponentModel;

namespace UntappdViewer.UI.Controls.Maps.GMapNet.ViewModel
{
    public class LocationConverter :TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType is string || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType is string || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string))
                return base.ConvertFrom(context, culture, value);

            try
            {
                string[] values = value.ToString().Split(',');
                if (values.Length == 0)
                    return new Location(0, 0);

                double latitude = Convert.ToDouble(values[0].Trim());
                if (values.Length == 1)
                    return new Location(latitude, latitude);

                double longitude = Convert.ToDouble(values[1].Trim());
                return new Location(latitude, longitude);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Cannot convert '{0}' ({1}) because {2}", value, value.GetType(), ex.Message), ex);
            }
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            Location gpoint = value as Location;

            if (gpoint != null && CanConvertTo(context, destinationType))
                return gpoint.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}