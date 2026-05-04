namespace M3U8Parser.Tags.MediaSegment
{
    using M3U8Parser.Attributes.Name;

    public class Map : AbstractTag
    {
        private readonly Uri _uri = new ();
        private readonly ByteRange _byteRange = new ();

        public Map()
        {
        }

        public Map(string str)
            : base(str)
        {
        }

        public string Uri {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public string ByteRange {
            get => _byteRange.Value;
            set => _byteRange.Value = value;
        }

        protected override string TagName => Tag.EXTXMAP;
    }
}