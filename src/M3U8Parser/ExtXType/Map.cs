namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;

    public class Map : BaseExtX
    {
        public const string Prefix = "#EXT-X-MAP";

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

        protected override string ExtPrefix => Prefix;
    }
}