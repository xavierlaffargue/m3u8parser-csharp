namespace M3U8Parser.Tags.Basic
{
    using M3U8Parser.Attributes.ValueType;

    public class Define : AbstractTag
    {
        private readonly StringAttribute _name = new ("NAME");
        private readonly StringAttribute _value = new ("VALUE");
        private readonly StringAttribute _import = new ("IMPORT");
        private readonly StringAttribute _queryParam = new ("QUERYPARAM");

        public Define()
        {
        }

        public Define(string str)
            : base(str)
        {
        }

        public string Name
        {
            get => _name.Value;
            set => _name.Value = value;
        }

        public string Value
        {
            get => _value.Value;
            set => _value.Value = value;
        }

        public string Import
        {
            get => _import.Value;
            set => _import.Value = value;
        }

        public string QueryParam
        {
            get => _queryParam.Value;
            set => _queryParam.Value = value;
        }

        protected override string TagName => Tag.EXTXDEFINE;
    }
}
