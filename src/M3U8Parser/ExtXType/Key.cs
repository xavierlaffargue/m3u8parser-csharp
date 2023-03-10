using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;

    public class Key : BaseExtX
    {
		private readonly Method _method = new ();
		private readonly Uri _uri = new ();

		public Key()
		{
		}

		public Key(string str) : base(str)
		{
        }

		public string Uri {
			get => _uri.Value;
			set => _uri.Value = value;
		}

		public MethodType Method {
			get => _method.Value;
			set => _method.Value = value;
		}

		public static string Prefix = "#EXT-X-KEY";

        protected override string ExtPrefix => Prefix;
    }
}