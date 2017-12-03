using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewVisualization.Converters
{
    class BitmapToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            try
            {
                var bitmap = (Bitmap) value;
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    bitmap.PixelFormat);

                var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96,
                    ConvertPixelFormat(bitmapData.PixelFormat), null,
                    bitmapData.Scan0, bitmapData.Stride*bitmapData.Height, bitmapData.Stride);

                bitmap.UnlockBits(bitmapData);

                return bitmapSource;
            }
            catch (Exception ex)
            {
                if (ex is NotImplementedException)
                    throw;
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
#pragma warning disable RECS0083 // Shows NotImplementedException throws in the quick task bar
            throw new NotImplementedException();
#pragma warning restore RECS0083 // Shows NotImplementedException throws in the quick task bar
        }

        private System.Windows.Media.PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat format)
        {
            switch (format)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return PixelFormats.Bgr24;

                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    return PixelFormats.Bgra32;

                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return PixelFormats.Bgr32;

                default:
#pragma warning disable RECS0083 // Shows NotImplementedException throws in the quick task bar
                    throw new NotImplementedException($"Pixel format {format} not implemented");
#pragma warning restore RECS0083 // Shows NotImplementedException throws in the quick task bar
            }
        }
    }
}
