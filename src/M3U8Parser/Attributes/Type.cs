namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class Type : CustomAttribute<MediaType>
    {
        public Type()
            : base("TYPE")
        {
        }
    }
}