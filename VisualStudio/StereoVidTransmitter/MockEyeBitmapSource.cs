using System;
using System.Collections.Generic;
using System.Drawing;

namespace UnityTransmitter
{
    public class MockEyeBitmapSource : IEyeBitmapSource
    {
        private readonly List<Bitmap> _bitmaps;
        public MockEyeBitmapSource()
        {
            _bitmaps = new List<Bitmap>
            {
                (Bitmap) Image.FromFile("//TestBitmaps//1test.jpg"),
                (Bitmap) Image.FromFile("//TestBitmaps//2test.jpg")
            };
        }
        public Bitmap GetLeftEyeBitmap()
        {
            return _bitmaps[0];
        }

        public Bitmap GetRightEyeBitmap()
        {
            return _bitmaps[1];
        }
    }
}