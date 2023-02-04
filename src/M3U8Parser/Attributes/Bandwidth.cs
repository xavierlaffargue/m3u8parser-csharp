namespace M3U8Parser.Attributes
{
    public class Bandwidth : CustomAttribute<long>
    {
        public Bandwidth() : base("BANDWIDTH")
        {
        }
    }
}