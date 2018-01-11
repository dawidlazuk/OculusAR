using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ViewProvision.Contract
{

    /// <summary>
    /// Used to transfer bitmaps as byte array.
    /// </summary>
    [DataContract]
    class BitmapDTO
    {
        [DataMember]
        private byte[] bytes;

        private Bitmap bitmap;

        public BitmapDTO(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            bytes = GetBytesFromBitmap(bitmap);
        }

        /// <summary>
        /// Get Bitmap from DTO.
        /// </summary>
        public Bitmap Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    if (bytes == null)
                        bitmap = null;
                    else
                    {
                        var bf = new BinaryFormatter();
                        using (var stream = new MemoryStream(bytes))
                        {
                            bitmap = (Bitmap)bf.Deserialize(stream);
                        }
                    }
                }
                return bitmap;
            }
        }

        public void Dispose()
        {
            bitmap?.Dispose();
        }

        [HandleProcessCorruptedStateExceptions]
        private byte[] GetBytesFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            try
            {
                using (var stream = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(stream, bitmap);
                    return stream.ToArray();
                }
            }
            catch(AccessViolationException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
