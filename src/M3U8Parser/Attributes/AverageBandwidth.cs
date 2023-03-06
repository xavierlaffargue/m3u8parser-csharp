using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser.Attributes
{
    public class AverageBandwidth: CustomAttribute<long?>
    {
        public AverageBandwidth() : base("AVERAGE-BANDWIDTH")
        {
        }
    }
}