using System.Runtime.Serialization;

namespace ViewProvision.Contract
{
    /// <summary>
    /// Details about the retrieved frames for both channels
    /// </summary>
    [DataContract]
    public class CaptureDetails
    {
        /// <summary>
        /// Left channel details
        /// </summary>
        [DataMember]
        public ChannelDetails LeftChannel { get; set; }

        /// <summary>
        /// Right channel details
        /// </summary>
        [DataMember]
        public ChannelDetails RightChannel { get; set; }
    }

    /// <summary>
    /// Details about the retrieved frame from a channel
    /// </summary>
    [DataContract]
    public class ChannelDetails
    {

        /// <summary>
        /// Index of the camera assigned 
        /// </summary>
        [DataMember]
        public int CaptureIndex { get; set; }


        /// <summary>
        /// Angle of the image rotation
        /// </summary>
        [DataMember]
        public int RotationAngle { get; set; }

        /// <summary>
        /// Horizontal resolution
        /// </summary>
        [DataMember]
        public int FrameWidth { get; set; }
        
        /// <summary>
        /// Vertical resolution
        /// </summary>
        [DataMember]
        public int FrameHeight { get; set; }
    }
}