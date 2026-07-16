namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class PartInf : AbstractTag
    {
        private readonly DecimalAttribute _partTarget = new ("PART-TARGET");

        public PartInf()
        {
        }

        public PartInf(string str)
            : base(str)
        {
        }

        public decimal? PartTarget
        {
            get => _partTarget.Value;
            set => _partTarget.Value = value;
        }

        protected override string TagName => Tag.EXTXPARTINF;
    }
}
