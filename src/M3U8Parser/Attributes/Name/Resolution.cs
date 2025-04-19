namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class Resolution : CustomAttribute<ResolutionType>
    {
        public Resolution()
            : base("RESOLUTION")
        {
        }
    }
}