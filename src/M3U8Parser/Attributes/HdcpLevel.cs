namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class HdcpLevel : CustomAttribute<HdcpLevelType>
    {
        public HdcpLevel()
            : base("HDCP-LEVEL")
        {
        }
    }
}