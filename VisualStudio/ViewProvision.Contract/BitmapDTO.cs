using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ViewProvision.Contract
{
    [DataContract]
    public class BitmapDTO
    {
        [DataMember]
        private byte[] bytes;

        private Bitmap bitmap;

        public BitmapDTO(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            bytes = GetBytesFromBitmap(bitmap);
        }

        public Bitmap Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    var bf = new BinaryFormatter();
                    using (var stream = new MemoryStream(bytes))
                    {
                        bitmap = (Bitmap)bf.Deserialize(stream);
                    }
                }
                return bitmap;
            }
        }

        public void Dispose()
        {
            bitmap?.Dispose();
        }

        private byte[] GetBytesFromBitmap(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(stream, bitmap);
                return stream.ToArray();
            }
        }
    }
}
