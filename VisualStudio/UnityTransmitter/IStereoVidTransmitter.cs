using UnityEngine;

namespace UnityTransmitter
{
    public interface IStereoVidTransmitter
    {
        Texture GetLeftEyeTexture();
        Texture GetRightEyeTexture();
    }
}
