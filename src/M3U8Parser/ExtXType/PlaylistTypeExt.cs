namespace M3U8Parser.ExtXType
{
	using M3U8Parser.CustomType;
	using System;
	using System.Text.RegularExpressions;

	public class PlaylistTypeExt : BaseExtX
	{
		public static string Prefix = "#EXT-X-PLAYLIST-TYPE";
		
		protected override string ExtPrefix => PlaylistTypeExt.Prefix;
		
		public PlaylistType Value { get; set; }
		
		public PlaylistTypeExt()
		{
			
		}
		
		public PlaylistTypeExt(string str)
		{
			Read(str);
		}
		
		public void Read(string content)
		{
			var match = Regex.Match(content.Trim(), $"(?<={Prefix}:)(.*?)(?=$)",
				RegexOptions.Multiline & RegexOptions.IgnoreCase);
    
			var type = typeof(PlaylistType);
                
			if (match.Success)
			{
				var valueFounded = match.Groups[0].Value;
    
				if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) 
				{
					type = Nullable.GetUnderlyingType(type);
				}
    				
				if (typeof(ICustomAttribute).IsAssignableFrom(type))
				{
					var instanceAttribute = (ICustomAttribute)Activator.CreateInstance(type, false);
					Value = (PlaylistType)instanceAttribute.ParseFromString(valueFounded);
				}
				else
				{
					Value = (PlaylistType)Convert.ChangeType(valueFounded, type);
				}
			}
			else
			{
				Value = default(PlaylistType);
			}
		}
	}
}