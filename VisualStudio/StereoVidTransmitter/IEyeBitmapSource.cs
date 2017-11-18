using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTransmitter
{
    public interface IEyeBitmapSource
    {
        Bitmap GetLeftEyeBitmap();
        Bitmap GetRightEyeBitmap();
    }
}
