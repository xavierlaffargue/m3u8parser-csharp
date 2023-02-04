using M3U8Parser.CustomType;

namespace M3U8Parser.Attributes
{
    public class Type : CustomAttribute<MediaType>
    {
        public Type() : base("TYPE")
        {
        }
    }
}