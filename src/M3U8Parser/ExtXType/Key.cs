namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;
    using M3U8Parser.Attributes.BaseAttribute;

    public class Key : BaseExtX
    {
        public const string Prefix = "#EXT-X-KEY";
        private readonly Method _method = new ();
        private readonly Uri _uri = new ();

        public Key()
        {
        }

        public Key(string str)
            : base(str)
        {
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public MethodType Method
        {
            get => _method.Value;
            set => _method.Value = value;
        }

        protected override string ExtPrefix => Prefix;
    }
}