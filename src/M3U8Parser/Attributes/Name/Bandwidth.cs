namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class Bandwidth : CustomAttribute<long>
    {
        public Bandwidth()
            : base("BANDWIDTH")
        {
        }
    }
}