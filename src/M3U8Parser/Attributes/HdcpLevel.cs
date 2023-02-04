using M3U8Parser.CustomType;

namespace M3U8Parser.Attributes
{
    public class HdcpLevel : CustomAttribute<HdcpLevelType>
    {
        public HdcpLevel() : base("HDCP-LEVEL")
        {
        }
    }
}