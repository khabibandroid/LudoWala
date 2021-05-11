using System.Xml.Serialization;

namespace Toast.Kit.Manager.Constant
{
    [XmlRoot("cdninfo")]
    public class CdnInfo
    {
        public string uri = string.Empty;
    }
}