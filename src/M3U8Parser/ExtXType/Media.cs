namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;
    using M3U8Parser.CustomType;
    using M3U8Parser.Extensions;
    using System.Text;

    public class Media : IExtXType
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
		{
			_language.Read(str);
			_name.Read(str);
			_mediaType.Read(str);
			_autoSelect.Read(str);
			_uri.Read(str);
			_default.Read(str);
			_groupId.Read(str);
			_instreamId.Read(str);
			_characteristics.Read(str);
		}

		public string Uri {
			get => _uri.Value;
			set => _uri.Value = value;
		}

		public string Language {
			get => _language.Value;
			set => _language.Value = value;
		}

		public string Name {
			get => _name.Value;
			set => _name.Value = value;
		}

		public bool AutoSelect {
			get => _autoSelect.Value;
			set => _autoSelect.Value = value;
		}

		public bool Default {
			get => _default.Value;
			set => _default.Value = value;
		}

		public string GroupId {
			get => _groupId.Value;
			set => _groupId.Value = value;
		}

		public string InstreamId {
			get => _instreamId.Value;
			set => _instreamId.Value = value;
		}

		public MediaType Type {
			get => _mediaType.Value;
			set => _mediaType.Value = value;
		}

		public string Characteristics {
			get => _characteristics.Value;
			set => _characteristics.Value = value;
		}

		public static string Prefix => "#EXT-X-MEDIA";

		public override string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
			strBuilder.AppendWithSeparator(_mediaType.ToString(), ",");
			strBuilder.AppendWithSeparator(_groupId.ToString(), ",");
			strBuilder.AppendWithSeparator(_name.ToString(), ",");
			strBuilder.AppendWithSeparator(_language.ToString(), ",");
			strBuilder.AppendWithSeparator(_autoSelect.ToString(), ",");
			strBuilder.AppendWithSeparator(_default.ToString(), ",");
			strBuilder.AppendWithSeparator(_instreamId.ToString(), ",");
			strBuilder.AppendWithSeparator(_characteristics.ToString(), ",");
			strBuilder.AppendWithSeparator(_uri.ToString(), ",");

			return strBuilder.ToString().RemoveLastCharacter();
		}
	}
}