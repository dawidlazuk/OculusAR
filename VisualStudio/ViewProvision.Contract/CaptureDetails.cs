using System.Runtime.Serialization;

namespace ViewProvision.Contract
{
    [DataContract]
    public class CaptureDetails
    {
        [DataMember]
        public int LeftIndex { get; set; }
        [DataMember]
        public int RightIndex { get; set; }
    }
}