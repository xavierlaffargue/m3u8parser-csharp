namespace SimpleM3u8Parser
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class MasterPlaylist
	{
		public List<Media> Medias = new ();
		public List<IframeStreamInf> IFrameStreams = new ();
		public List<StreamInf> Streams = new ();

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
				
				if(line.StartsWith(IframeStreamInf.Prefix))
				{
					IframeStreamInf streaminf = new IframeStreamInf(line);
					IFrameStreams.Add(streaminf);
				}
				
				if(line.StartsWith(StreamInf.Prefix))
				{
					StreamInf stream = new StreamInf(line);
					Streams.Add(stream);
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
		public static string Prefix { get => "#EXT-X-STREAM-INF"; }

		private Attribute<int> _programId { get; } = new ("PROGRAM-ID");

		private Attribute<long> Bandwidth { get; } = new ("BANDWIDTH");

		private Attribute<string> Codecs { get; } = new ("CODECS");
		
		private Attribute<string> Video { get; } = new ("VIDEO");

		private readonly Attribute<string> _audio = new ("AUDIO");

		public string Audio 
		{
			get
			{
				return _audio.Value;
			}
			set
			{
				_audio.Value = value;
			}
		}  
		
		private string UriSingleLine;
		
		public StreamInf(string str)
		{
			string lineWithAttribute = string.Empty;
			string lineWithUri = string.Empty;
			
			using var reader = new StringReader(str);
			lineWithAttribute = reader.ReadLine();
			lineWithUri = reader.ReadLine();

			_programId.Read(lineWithAttribute);
			Bandwidth.Read(lineWithAttribute);
			Codecs.Read(lineWithAttribute);
			Video.Read(lineWithAttribute);
			_audio.Read(lineWithAttribute);
			UriSingleLine = lineWithUri;
		}

		public new string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
			strBuilder.AppendSeparatorIfTextNotNull(_programId.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Bandwidth.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Codecs.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Video.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(_audio.ToString(), ",");

			return strBuilder.ToString().RemoveLastCharacter() + "\r\n" + UriSingleLine;
		}
	}

	public class IframeStreamInf : IExtXType
    {
	    public static string Prefix { get => "#EXT-X-I-FRAME-STREAM-INF"; }
	    
	    private Attribute<string> Uri { get; } = new ("URI");
	    
	    private Attribute<long> Bandwidth { get; } = new ("BANDWIDTH");
	   
	    private Resolution Resolution { get; } = new ();
	    
	    private Attribute<string> Codecs { get; } = new ("CODECS");
	    
	    public IframeStreamInf() {}

	    public IframeStreamInf(string str)
	    {
		    Bandwidth.Read(str);
		    Resolution.Read(str);
		    Codecs.Read(str);
		    Uri.Read(str); 
	    }

	    public new string ToString()
	    {
		    var strBuilder = new StringBuilder();
		    strBuilder.Append(Prefix);
		    strBuilder.Append(":");
		    strBuilder.AppendSeparatorIfTextNotNull(Bandwidth.ToString(), ",");
		    strBuilder.AppendSeparatorIfTextNotNull(Resolution.ToString(), ",");
		    strBuilder.AppendSeparatorIfTextNotNull(Codecs.ToString(), ",");
		    strBuilder.AppendSeparatorIfTextNotNull(Uri.ToString(), ",");
            
		    return strBuilder.ToString().RemoveLastCharacter();
	    }
	}
	
	public class Media : IExtXType
    {
	    public static string Prefix { get => "#EXT-X-MEDIA"; }

        private Attribute<string> Language { get; } = new ("LANGUAGE");
        
        private Attribute<string> Name { get; } = new ("NAME");

        private Attribute<string> Uri { get; } = new ("URI");
        
        private Attribute<bool> AutoSelect { get; } = new ("AUTOSELECT");
        
        private Attribute<bool> Default { get; } = new ("DEFAULT");

        private TypeAttribut MediaType { get; } = new (SimpleM3u8Parser.MediaType.NONE);
        
        private Attribute<string> GroupId { get; } = new ("GROUP-ID");
        
        private Attribute<string> InstreamId { get; } = new ("INSTREAM-ID");

        public Media()
		{}
		
		public Media(string str)
		{
			Language.Read(str);
			Name.Read(str);
			MediaType.Read(str);
			AutoSelect.Read(str);
			Uri.Read(str);
			Default.Read(str);
			GroupId.Read(str);
			InstreamId.Read(str);
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
        
        public string GetUri()
        {
	        return Uri.Value;
        }

		public Media SetType(MediaType mediaType)
		{
			MediaType.Value = mediaType;
			return this;
        }
		
		public Media SetUri(string uri)
		{
			Uri.Value = uri;
			return this;
		}

		public new string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
			strBuilder.AppendSeparatorIfTextNotNull(MediaType.Parse(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(GroupId.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Language.ToString(), ",");
			strBuilder.AppendSeparatorIfTextNotNull(Name.ToString(), ",");
            strBuilder.AppendSeparatorIfTextNotNull(AutoSelect.ToString(), ",");
            strBuilder.AppendSeparatorIfTextNotNull(Default.ToString(), ",");
            strBuilder.AppendSeparatorIfTextNotNull(Uri.ToString(), ",");
            
            return strBuilder.ToString().RemoveLastCharacter();
		}
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
		NONE,AUDIO,VIDEO,CLOSEDCAPTIONS
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
                return;
            }
        }
    }



	public class Resolution : Attribute<string>
	{
		private readonly string separator = "=";
		
		public Resolution() : base("RESOLUTION")
		{
		}

		public override string ToString()
		{
			if(Value != null)
			{
				return $"{AttributeName}{separator}{Value}";
			}

			return string.Empty;
		}
	}

    public class Attribute<T>
    {
        private readonly string separator = "=";
        public string AttributeName { get; }
        public T Value { get; set; }

        public Attribute(string attributeName)
        {
            this.AttributeName = attributeName;
        }

        public override string ToString()
        {
	        if(Value != null)
	        {
		        string textValue = $"{Value}";
		        if (Value is bool boolValue)
		        {
			        textValue = BoolToString(boolValue);
		        }
		        else if (Value is string stringValue)
		        {
			        textValue = $"\"{textValue}\"";
		        }
		        
                return $"{AttributeName}{separator}{textValue}";
            }

            return string.Empty;
        }

        private string BoolToString(bool value)
        {
	        if (value)
	        {
		        return "YES";
	        }

	        return "NO";
        }

        private bool StringToBool(string value)
        {
	        if (value == "YES")
	        {
		        return true;
	        }

	        return false;
        }
        
        
        public void Read(string content)
        {
	        var regexStr = Regex.Match(content.Trim(), $"(?<={AttributeName}=\")(.*?)(?=\",|\"$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
	        if(regexStr.Success)
	        {
		        var valueFounded =  regexStr.Groups[0].Value;
		        Value = (T)Convert.ChangeType(valueFounded, typeof(T));
		        return;
	        }
	        
            var match = Regex.Match(content.Trim(), $"(?={AttributeName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            if (match.Success)
            {
	            var valueFounded = match.Groups[0].Value.Split("=")[1];

	            if (typeof(T) == typeof(bool))
	            {
		            Value = (T)Convert.ChangeType(StringToBool(valueFounded), typeof(T));
	            }
	            else
	            {
		            Value = (T)Convert.ChangeType(valueFounded, typeof(T));

	            }
            }
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