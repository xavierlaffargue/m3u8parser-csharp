namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class AverageBandwidth : CustomAttribute<long?>
    {
        public AverageBandwidth()
            : base("AVERAGE-BANDWIDTH")
        {
        }
    }
}