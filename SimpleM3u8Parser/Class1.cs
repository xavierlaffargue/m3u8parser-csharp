namespace SimpleM3u8Parser
{
	using System.Text.RegularExpressions;

	public class Media
	{
		public const string Prefix = "#EXT-X-MEDIA";
		
		private LanguageAttribut Language { get; } = new LanguageAttribut();
		private NameAttribut Name { get; } = new NameAttribut();
		
		public Media()
		{}
		
		public Media(string str)
		{
			Language.Read(str);
			Name.Read(str);
		}
		
		public Media SetLanguage(string l)
		{
			Language.Value = l;
			return this;
		}
		
		public Media SetName(string l)
		{
			Name.Value = l;
			return this;
		}


		public string GetLanguage()
		{
			return Language.Value;
		}
		
		public string GetName()
		{
			return Name.Value;
		}

		public new string ToString()
		{
			return Prefix + ":" + Language.Parse() + "," + Name.Parse();
		}
	}

	public class LanguageAttribut : StringTag
	{
		public override string AttributName => "LANGUAGE";
	}
	
	public class NameAttribut : StringTag
	{
		public override string AttributName => "NAME";
	}
	
	public abstract class StringTag
	{
		private string Separator = "=";
		public abstract string AttributName { get; }
		public string Value = "";
		
		public virtual string Parse()
		{
			return AttributName + Separator + '"' + Value  + '"';
		}
	
		public void Read(string content)
		{
			var regex = new Regex($"(?={AttributName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
			if (regex.IsMatch(content))
			{
				Value = regex.Match(content).Groups[0].Value.Split(Separator)[1].Replace("\"", "");
			}
		}
	}
}