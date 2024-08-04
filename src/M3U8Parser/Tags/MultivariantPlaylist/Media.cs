namespace M3U8Parser.Tags.MultivariantPlaylist
{
    using M3U8Parser.Attributes.Name;
    using M3U8Parser.Attributes.ValueType;

    public class Media : AbstractTag
    {
        private readonly Autoselect _autoSelect = new ();
        private readonly Characteristics _characteristics = new ();
        private readonly Default _default = new ();
        private readonly GroupId _groupId = new ();
        private readonly InstreamId _instreamId = new ();
        private readonly Language _language = new ();
        private readonly Type _mediaType = new ();
        private readonly Name _name = new ();
        private readonly Uri _uri = new ();

        public Media()
        {
        }

        public Media(string str)
            : base(str)
        {
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public string Language
        {
            get => _language.Value;
            set => _language.Value = value;
        }

        public string Name
        {
            get => _name.Value;
            set => _name.Value = value;
        }

        public bool AutoSelect
        {
            get => _autoSelect.Value;
            set => _autoSelect.Value = value;
        }

        public bool Default
        {
            get => _default.Value;
            set => _default.Value = value;
        }

        public string GroupId
        {
            get => _groupId.Value;
            set => _groupId.Value = value;
        }

        public string InstreamId
        {
            get => _instreamId.Value;
            set => _instreamId.Value = value;
        }

        public MediaType Type
        {
            get => _mediaType.Value;
            set => _mediaType.Value = value;
        }

        public string Characteristics
        {
            get => _characteristics.Value;
            set => _characteristics.Value = value;
        }

        protected override string TagName => Tag.EXTXMEDIA;
    }
}