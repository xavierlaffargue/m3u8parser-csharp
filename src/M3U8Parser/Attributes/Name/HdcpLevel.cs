namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class HdcpLevel : CustomAttribute<HdcpLevelType>
    {
        public HdcpLevel()
            : base("HDCP-LEVEL")
        {
        }
    }
}