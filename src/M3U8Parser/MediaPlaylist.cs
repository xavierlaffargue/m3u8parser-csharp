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
		public PlaylistType? PlaylistType { get; set; } = new ();
		
		public int HlsVersion { get; set; }

        public bool? IFrameOnly { get; set; }

        public int? TargetDuration { get; set; }

        public int? MediaSequence { get; set; }

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
			PlaylistType playlistType = null;
			List<MediaSegment> mediaSegments = new ();
			var hlsVersion = 4;
			var hasEndList = false;
			int? targetDuration = null;
            int? mediaSequence = null;
			bool? iFrameOnly = null;
            //var l = Regex.Split(text, "(?=#EXT-X)");

            //var l = Regex.Match(text, "(?=#EXT-X)");

            Regex regex = new Regex("(?=#EXT-X)(.*?)(?<=$)", RegexOptions.Multiline);
			var matches = regex.Matches(text);

            foreach (Match match in matches)
			{
				var line = match.Value;
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
				else if(line.StartsWith("#EXT-X-I-FRAMES-ONLY"))
				{
                    iFrameOnly = true;
                }
				else if(line.StartsWith("#EXT-X-TARGETDURATION"))
				{
                    targetDuration = new TargetDuration(line).Value;
                }
                else if (line.StartsWith(M3U8Parser.ExtXType.MediaSequence.Prefix))
                {
                    mediaSequence = new MediaSequence(line).Value;

                }
            }
            
            var l = Regex.Split(text, $"(?=#EXT-X-KEY|#EXTINF)");
		//	var l = Regex.Split(text, $"(?={MediaSegment.Prefix})");

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
				HasEndList = hasEndList,
                TargetDuration = targetDuration,
				MediaSequence = mediaSequence,
                IFrameOnly = iFrameOnly
            };
		}

		public override string ToString()
		{
			var strBuilder = new StringBuilder();

			strBuilder.AppendLine("#EXTM3U");

			strBuilder.AppendLine($"#EXT-X-VERSION:{HlsVersion}");

			if (TargetDuration != null)
			{
				strBuilder.AppendLine($"#EXT-X-TARGETDURATION:{TargetDuration}");
			}

			if (MediaSequence != null)
			{
				strBuilder.AppendLine($"#EXT-X-MEDIA-SEQUENCE:{MediaSequence}");
			}

			if (PlaylistType != null)
			{
				strBuilder.AppendLine($"#EXT-X-PLAYLIST-TYPE:{PlaylistType}");
			}

            if (IFrameOnly.HasValue && IFrameOnly.Value)
            {
                strBuilder.AppendLine($"#EXT-X-I-FRAMES-ONLY");
            }

            strBuilder.AppendLine();

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