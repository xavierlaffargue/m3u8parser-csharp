using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace M3U8Parser
{
	using M3U8Parser.CustomType;
	using M3U8Parser.ExtXType;

    public class MediaPlaylist
	{
		public PlaylistType PlaylistType { get; set; } = new ();

		public List<MediaSegment> MediaSegments { get; private set; } = new ();
		
		public MediaPlaylist()
		{
		}
		

		public static MediaPlaylist LoadFromFile(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"File not found : {path}");
			}

			return LoadFromText(File.ReadAllText(path));
		}
		
		public static MediaPlaylist LoadFromText(string text)
		{
			PlaylistType playlistType = new ();
			List<MediaSegment> mediaSegments = new ();
			/*var hlsVersion = 4;

			var matchHlsVersion = Regex.Match(text, "(?<=#EXT-X-VERSION:)(.*?)(?<=$)", RegexOptions.Multiline);
			if (matchHlsVersion.Success)
			{
				var info = matchHlsVersion.Groups[0].Value;
				hlsVersion = int.Parse(info);
			}*/


			var l = Regex.Split(text, "(?=#EXT)");

			foreach (var line in l)
			{
				if (line.StartsWith(PlaylistTypeExt.Prefix))
				{
					playlistType = new PlaylistTypeExt(line).Value;
				}
				else if (line.StartsWith(MediaSegment.Prefix))
				{
					mediaSegments.Add(new MediaSegment(line));
				}
			}

			return new MediaPlaylist()
			{
				PlaylistType = playlistType
			};
		}

		public override string ToString()
		{
			var strBuilder = new StringBuilder();

			strBuilder.AppendLine("#EXTM3U");

			strBuilder.AppendLine();
			

			return strBuilder.ToString().TrimEnd();
		}
	}
}