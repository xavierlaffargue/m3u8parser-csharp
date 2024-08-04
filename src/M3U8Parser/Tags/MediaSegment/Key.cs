namespace M3U8Parser.Tags.MediaSegment
{
    using M3U8Parser.Attributes.Name;
    using M3U8Parser.Attributes.ValueType;

    public class Key : AbstractTag
    {
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

        protected override string TagName => Tag.EXTXKEY;
    }
}