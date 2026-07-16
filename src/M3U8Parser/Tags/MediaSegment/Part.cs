namespace M3U8Parser.Tags.MediaSegment
{
    using M3U8Parser.Attributes.ValueType;

    public class Part : AbstractTag
    {
        private readonly StringAttribute _uri = new ("URI");
        private readonly DecimalAttribute _duration = new ("DURATION");
        private readonly BoolAttribute _independent = new ("INDEPENDENT");
        private readonly StringAttribute _byterange = new ("BYTERANGE");
        private readonly BoolAttribute _gap = new ("GAP");

        public Part()
        {
        }

        public Part(string str)
            : base(str)
        {
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public decimal? Duration
        {
            get => _duration.Value;
            set => _duration.Value = value;
        }

        public bool Independent
        {
            get => _independent.Value;
            set => _independent.Value = value;
        }

        public string ByteRange
        {
            get => _byterange.Value;
            set => _byterange.Value = value;
        }

        public bool Gap
        {
            get => _gap.Value;
            set => _gap.Value = value;
        }

        protected override string TagName => Tag.EXTXPART;
    }
}
