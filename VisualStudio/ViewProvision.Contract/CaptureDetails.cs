using System.Runtime.Serialization;

namespace ViewProvision.Contract
{
    [DataContract]
    public class CaptureDetails
    {
        [DataMember]
        public ChannelDetails LeftChannel { get; set; }
        [DataMember]
        public ChannelDetails RightChannel { get; set; }
    }

    [DataContract]
    public class ChannelDetails
    {
        [DataMember]
        public int CaptureIndex { get; set; }

        [DataMember]
        public int RotationAngle { get; set; }

        [DataMember]
        public int FrameWidth { get; set; }

        [DataMember]
        public int FrameHeight { get; set; }
    }
}