using System;

namespace ViewVisualization
{
    [Obsolete]
    public static class BitmapSourceConvert
    {
        //[DllImport("gdi32")]
        //private static extern int DeleteObject(IntPtr o);

        //public static BitmapSource ToBitmapSource(IImage image)
        //{
        //    if (image == null)
        //        return null; //TODO set image as sth like "no image captured"
        //    try
        //    {
        //        using (System.Drawing.Bitmap source = image.Bitmap)
        //        {
        //            IntPtr ptr = source.GetHbitmap();

        //            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //                ptr,
        //                IntPtr.Zero,
        //                Int32Rect.Empty,
        //                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

        //            DeleteObject(ptr);
        //            return bs;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
    }
}
