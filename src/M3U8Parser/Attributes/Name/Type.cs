namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class Type : CustomAttribute<MediaType>
    {
        public Type()
            : base("TYPE")
        {
        }
    }
}