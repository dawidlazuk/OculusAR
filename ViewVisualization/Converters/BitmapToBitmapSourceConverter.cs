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

            //try
            //{
            //    var bitmap = (Bitmap) value;
            //    MemoryStream stream = new MemoryStream();

            //    bitmap.Save(stream, ImageFormat.Bmp);
            //    GC.KeepAlive(stream);
            //    stream.Position = 0;
            //    BitmapImage bitmapImage = new BitmapImage();
            //    bitmapImage.BeginInit();
            //    bitmapImage.StreamSource = stream;
            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapImage.EndInit();

            //    return bitmapImage;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    return null;
            //}

            var bitmap = (Bitmap) value;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, ConvertPixelFormat(bitmapData.PixelFormat), null,
                bitmapData.Scan0, bitmapData.Stride*bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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
                    throw new NotImplementedException($"Pixel format {format} not implemented");
            }
        }
    }
}
