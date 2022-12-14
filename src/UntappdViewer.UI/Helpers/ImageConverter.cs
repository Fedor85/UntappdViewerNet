using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace UntappdViewer.UI.Helpers
{
    public static class ImageConverter
    {
        public static BitmapSource GetBitmapSource(string imagePath)
        {
            using (Bitmap image = new Bitmap(imagePath))
                return GetBitmapSource(image);
        }

        public static BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static BitmapSource GerBitmapSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}