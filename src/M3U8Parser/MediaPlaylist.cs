using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace M3U8Parser
{
	using M3U8Parser.CustomType;
	using M3U8Parser.ExtXType;
	using System.Runtime.CompilerServices;

	public class MediaPlaylist
	{
		public PlaylistType PlaylistType { get; set; } = new ();
		
		public int HlsVersion { get; set; }

		public List<MediaSegment> MediaSegments { get; private set; } = new ();
		
		public bool HasEndList { get; set; }
		
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
			var hlsVersion = 4;
			var hasEndList = false;
			
			var l = Regex.Split(text, "(?=#EXT-X)");

			foreach (var line in l)
			{
				if (line.StartsWith(PlaylistTypeExt.Prefix))
				{
					playlistType = new PlaylistTypeExt(line).Value;
				}
				else if (line.StartsWith(ExtXType.HlsVersion.Prefix))
				{
					hlsVersion = new HlsVersion(line).Value;
				}
				else if (line.StartsWith("#EXT-X-ENDLIST"))
				{
					hasEndList = true;
				}
			}
			
			l = Regex.Split(text, $"(?={MediaSegment.Prefix})");

			foreach (var line in l)
			{
				if (line.StartsWith(MediaSegment.Prefix))
				{
					mediaSegments.Add(new MediaSegment(line));
				}
			}

			return new MediaPlaylist()
			{
				PlaylistType = playlistType,
				HlsVersion = hlsVersion,
				MediaSegments = mediaSegments,
				HasEndList = hasEndList
			};
		}

		public override string ToString()
		{
			var strBuilder = new StringBuilder();

			strBuilder.AppendLine("#EXTM3U");

			strBuilder.AppendLine($"#EXT-X-VERSION:{HlsVersion}");
			strBuilder.AppendLine($"#EXT-X-PLAYLIST-TYPE:{PlaylistType}");

			foreach (var segment in MediaSegments)
			{
				strBuilder.Append(segment);
			}

			if (HasEndList)
			{
				strBuilder.AppendLine($"#EXT-X-ENDLIST");
			}

			return strBuilder.ToString().TrimEnd();
		}
	}
}