namespace M3U8Parser.Tags.MultivariantPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class ContentSteering : AbstractTag
    {
        private readonly StringAttribute _serverUri = new ("SERVER-URI");
        private readonly StringAttribute _pathwayId = new ("PATHWAY-ID");

        public ContentSteering()
        {
        }

        public ContentSteering(string str)
            : base(str)
        {
        }

        public string ServerUri
        {
            get => _serverUri.Value;
            set => _serverUri.Value = value;
        }

        public string PathwayId
        {
            get => _pathwayId.Value;
            set => _pathwayId.Value = value;
        }

        protected override string TagName => Tag.EXTXCONTENTSTEERING;
    }
}
