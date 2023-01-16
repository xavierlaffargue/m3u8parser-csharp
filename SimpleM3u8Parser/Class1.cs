namespace SimpleM3u8Parser
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class MasterPlaylist
	{
		public List<Media> Medias = new List<Media>();
		public List<StreamInf> StreamInfs = new List<StreamInf>();

		public MasterPlaylist()
		{}

		public MasterPlaylist(string text)
		{
			var l = Regex.Split(text, "(?=#EXT-X)");

            foreach (var line in l)
			{

				if(line.StartsWith(Media.Prefix))
				{
					Media media = new Media(line);
					Medias.Add(media);
				}
			}
        }

	}

	public interface IExtXType
	{
		public static string Prefix { get; }
	}

	public class StreamInf : IExtXType
    {
        public static string Prefix => "#EXT-X-STREAM-INF";
    }

	public class Media : IExtXType
    {
        public static string Prefix => "#EXT-X-MEDIA";

        private Attribute<string> Language { get; } = new Attribute<string>("LANGUAGE");

     //   private LanguageAttribut Language { get; } = new LanguageAttribut();
		private NameAttribut Name { get; } = new NameAttribut();
        private TypeAttribut MediaType { get; } = new TypeAttribut(SimpleM3u8Parser.MediaType.NONE);
        private AutoSelectAttribut AutoSelect { get; } = new AutoSelectAttribut();

		

        public Media()
		{}
		
		public Media(string str)
		{
			Language.Read(str);
			Name.Read(str);
			MediaType.Read(str);
			AutoSelect.Read(str);
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

        public Media SetAutoSelect(bool l)
        {
            AutoSelect.Value = l;
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

        public string GetAutoSelect()
        {
            return AutoSelect.Value.ToString();
        }

        public string GetMediaType()
		{
			return MediaType.Value.ToString();
		}

		public Media SetType(MediaType mediaType)
		{
			MediaType.Value = mediaType;
			return this;
        }

		public new string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
			strBuilder.AppendSeparatorIfTextNotNull(MediaType.Parse(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Language.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Name.Parse(), ",");
            strBuilder.AppendSeparatorIfTextNotNull(AutoSelect.Parse(), ",");

            return strBuilder.ToString().RemoveLastCharacter();
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

    public class AutoSelectAttribut : BoolTag
    {
        public override string AttributName => "AUTOSELECT";
    }

    public class TypeAttribut : EnumTag<MediaType>
    {
        public override string AttributName => "TYPE";

        public TypeAttribut(MediaType defaultValue) : base(defaultValue)
        {
        }
    }

	public enum MediaType
	{
		NONE,AUDIO,VIDEO
	}

    public abstract class StringTag
	{
		private string Separator = "=";
		public abstract string AttributName { get; }
		public string Value = "";
		
		public virtual string Parse()
		{
			if (!string.IsNullOrEmpty(Value))
			{
				return AttributName + Separator + '"' + Value + '"';
			}

			return string.Empty;
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


    public abstract class BoolTag
    {
        private string Separator = "=";
        public abstract string AttributName { get; }
        public bool Value;

        public virtual string Parse()
        {
			var valueStr = "YES";
            if (Value == false)
            {
				valueStr = "NO";
            }

            return AttributName + Separator + valueStr;
        }

        public void Read(string content)
        {
            var regex = new Regex($"(?={AttributName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
			var valueStr = string.Empty;
            if (regex.IsMatch(content))
            {
                valueStr = regex.Match(content).Groups[0].Value.Split(Separator)[1].Replace("\"", "");
            }

			if(valueStr == "YES")
			{
				Value = true;
			}
			else if(valueStr == "NO")
			{
				Value = false;
			}

        }

        public override string ToString()
        {
            if (Value)
            {
				return "YES";
            }

			return "NO";
        }
    }

    public abstract class EnumTag<T> where T : struct, IConvertible
    {
        private string Separator = "=";
        public abstract string AttributName { get; }
		private readonly T defaultValue;
        public T Value;

		public EnumTag(T defaultValue)
		{
			this.defaultValue = defaultValue;
			Value = defaultValue;
		}

        public virtual string Parse()
        {
			if (!Value.Equals(defaultValue))
			{
				return AttributName + Separator + Value;
			}

			return string.Empty;
        }

        public void Read(string content)
        {
            var regex = new Regex($"(?={AttributName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            if (regex.IsMatch(content))
            {
                Value = Tools.ParseEnum<T>(regex.Match(content).Groups[0].Value.Split(Separator)[1]);
            }
        }
    }




    public class Attribute<T>
    {
        private string separator = "=";
        public string AttributeName { get; }
        public T Value { get; set; }

		private T DefaultValue;
        public Attribute(string attributeName)
        {
            this.AttributeName = attributeName;
        }
        public Attribute(string attributeName, T defaultValue)
        {
            this.AttributeName = attributeName;
			DefaultValue = defaultValue;
            this.Value = defaultValue;
        }

        public override string ToString()
        {
			if(Value != null && !Value.Equals(DefaultValue))
            {
                return $"{AttributeName}{separator}{Value},";
            }

            return string.Empty;
        }

        public void Read(string content)
        {
            var match = Regex.Match(content, $"(?={AttributeName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            if (match.Success)
                Value = (T)Convert.ChangeType(match.Groups[0].Value.Split("=")[1], typeof(T));
        }
    }

    public static class Tools
	{
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

	public static class StringBuilderExtension
	{
		public static StringBuilder AppendSeparatorIfTextNotNull(this StringBuilder strBuilder, string text, string separator)
		{
            if(!string.IsNullOrEmpty(text))
			{
				strBuilder.Append(text + separator);
			}
			
			return strBuilder;
        }
	}

	public static class StringExtension
	{
		public static string RemoveLastCharacter(this string str)
		{
			return str.Remove(str.Length- 1, 1);
		}
	}
}