using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser.Attributes
{
    public class Resolution : CustomAttribute<ResolutionType>
    {
        public Resolution() : base("RESOLUTION")
        {
        }
    }
}