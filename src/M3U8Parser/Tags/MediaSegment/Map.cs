namespace M3U8Parser.Tags.MediaSegment
{
    using M3U8Parser.Attributes.Name;

    public class Map : AbstractTag
    {
        private readonly Uri _uri = new ();

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

        protected override string TagName => Tag.EXTXMAP;
    }
}