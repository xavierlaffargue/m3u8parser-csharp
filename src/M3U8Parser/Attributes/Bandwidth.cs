namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class Bandwidth : CustomAttribute<long>
    {
        public Bandwidth()
            : base("BANDWIDTH")
        {
        }
    }
}