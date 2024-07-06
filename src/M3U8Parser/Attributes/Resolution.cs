namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class Resolution : CustomAttribute<ResolutionType>
    {
        public Resolution()
            : base("RESOLUTION")
        {
        }
    }
}