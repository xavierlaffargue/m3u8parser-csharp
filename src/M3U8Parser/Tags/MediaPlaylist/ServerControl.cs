namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class ServerControl : AbstractTag
    {
        private readonly DecimalAttribute _canSkipUntil = new ("CAN-SKIP-UNTIL");
        private readonly BoolAttribute _canSkipDateRanges = new ("CAN-SKIP-DATERANGES");
        private readonly DecimalAttribute _holdBack = new ("HOLD-BACK");
        private readonly DecimalAttribute _partHoldBack = new ("PART-HOLD-BACK");
        private readonly BoolAttribute _canBlockReload = new ("CAN-BLOCK-RELOAD");

        public ServerControl()
        {
        }

        public ServerControl(string str)
            : base(str)
        {
        }

        public decimal? CanSkipUntil
        {
            get => _canSkipUntil.Value;
            set => _canSkipUntil.Value = value;
        }

        public bool CanSkipDateRanges
        {
            get => _canSkipDateRanges.Value;
            set => _canSkipDateRanges.Value = value;
        }

        public decimal? HoldBack
        {
            get => _holdBack.Value;
            set => _holdBack.Value = value;
        }

        public decimal? PartHoldBack
        {
            get => _partHoldBack.Value;
            set => _partHoldBack.Value = value;
        }

        public bool CanBlockReload
        {
            get => _canBlockReload.Value;
            set => _canBlockReload.Value = value;
        }

        protected override string TagName => Tag.EXTXSERVERCONTROL;
    }
}
