using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser
{
	using M3U8Parser.ExtXType;

	public class MediaPlaylist
	{
		public PlaylistType PlaylistType { get; set; } = new ();
		
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
				else if(line.StartsWith(ExtXType.TargetDuration.Prefix))
				{
                    targetDuration = new TargetDuration(line).Value;
                }
                else if (line.StartsWith(M3U8Parser.ExtXType.MediaSequence.Prefix))
                {
                    mediaSequence = new MediaSequence(line).Value;

                }
            }
            
            var l = Regex.Split(text, $"(?=#EXT-X-KEY|#EXTINF)");
			var segments = new List<Segment>();
            MediaSegment mediaSegment = null;
			foreach (var line in l)
			{
                if (line.StartsWith(Key.Prefix))
                {
                    if(mediaSegment != null)
                    {
						mediaSegment.Segments = segments;
						mediaSegments.Add(mediaSegment);
					}

                    mediaSegment = new MediaSegment(line);
                }

                if (line.StartsWith(Segment.Prefix))
				{
					if (mediaSegment == null)
					{
						mediaSegment= new MediaSegment();
					}

					segments.Add(new Segment(line));
				}
			}

            mediaSegment.Segments = segments;
            mediaSegments.Add(mediaSegment);

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

            foreach (var segment in MediaSegments)
			{
				strBuilder.AppendLine();
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