namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class AverageBandwidth : CustomAttribute<long?>
    {
        public AverageBandwidth()
            : base("AVERAGE-BANDWIDTH")
        {
        }
    }
}