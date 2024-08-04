namespace M3U8Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.Attributes.ValueType;
    using M3U8Parser.Tags.Basic;
    using M3U8Parser.Tags.MediaPlaylist;
    using M3U8Parser.Tags.MediaSegment;

    public class MediaPlaylist
    {
        public PlaylistType PlaylistType { get; set; } = new ();

        public Map Map { get; set; } = new ();

        public int HlsVersion { get; set; }

        public bool IndependentSegments { get; set; }

        public bool? IFrameOnly { get; set; }

        public int? TargetDuration { get; set; }

        public int? MediaSequence { get; set; }

        public List<MediaSegment> MediaSegments { get; set; } = new ();

        public bool HasEndList { get; set; }

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
            Map map = null;
            List<MediaSegment> mediaSegments = new ();
            var hlsVersion = 4;
            var hasEndList = false;
            int? targetDuration = null;
            int? mediaSequence = null;
            bool? iFrameOnly = null;
            bool independentSegments = false;

            var regex = new Regex($"(?={Tag.EXTX})(.*?)(?<=$)", RegexOptions.Multiline);
            var matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                var line = match.Value;
                if (line.StartsWith(Tag.EXTXINDEPENDENTSEGMENTS))
                {
                    independentSegments = true;
                }
                else if (line.StartsWith(Tag.EXTXPLAYLISTTYPE))
                {
                    playlistType = new PlaylistTypeExt(line).Value;
                }
                else if (line.StartsWith(Tag.EXTXVERSION))
                {
                    hlsVersion = new HlsVersion(line).Value;
                }
                else if (line.StartsWith(Tag.EXTXMAP))
                {
                    map = new Map(line);
                }
                else if (line.StartsWith(Tag.EXTXENDLIST))
                {
                    hasEndList = true;
                }
                else if (line.StartsWith(Tag.EXTXIFRAMESONLY))
                {
                    iFrameOnly = true;
                }
                else if (line.StartsWith(Tag.EXTXTARGETDURATION))
                {
                    targetDuration = new TargetDuration(line).Value;
                }
                else if (line.StartsWith(Tag.EXTXMEDIASEQUENCE))
                {
                    mediaSequence = new MediaSequence(line).Value;
                }
            }

            var l = Regex.Split(text, $"(?={Tag.EXTXKEY}|{Tag.EXTINF})");
            var segments = new List<Segment>();
            MediaSegment mediaSegment = null;
            foreach (var line in l)
            {
                if (line.StartsWith(Tag.EXTXKEY))
                {
                    if (mediaSegment != null)
                    {
                        mediaSegment.Segments = segments;
                        mediaSegments.Add(mediaSegment);
                    }

                    mediaSegment = new MediaSegment(line);
                }

                if (line.StartsWith(Tag.EXTINF))
                {
                    mediaSegment ??= new MediaSegment();

                    segments.Add(new Segment(line));
                }
            }

            mediaSegment.Segments = segments;
            mediaSegments.Add(mediaSegment);

            return new MediaPlaylist
            {
                PlaylistType = playlistType,
                HlsVersion = hlsVersion,
                MediaSegments = mediaSegments,
                HasEndList = hasEndList,
                TargetDuration = targetDuration,
                MediaSequence = mediaSequence,
                IFrameOnly = iFrameOnly,
                Map = map,
                IndependentSegments = independentSegments
            };
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine(Tag.EXTM3U);
            strBuilder.AppendLine($"{Tag.EXTXVERSION}:{HlsVersion}");

            if (IndependentSegments)
            {
                strBuilder.AppendLine(Tag.EXTXINDEPENDENTSEGMENTS);
            }

            if (TargetDuration != null)
            {
                strBuilder.AppendLine($"{Tag.EXTXTARGETDURATION}:{TargetDuration}");
            }

            if (MediaSequence != null)
            {
                strBuilder.AppendLine($"{Tag.EXTXMEDIASEQUENCE}:{MediaSequence}");
            }

            if (PlaylistType != null)
            {
                strBuilder.AppendLine($"{Tag.EXTXPLAYLISTTYPE}:{PlaylistType}");
            }

            if (IFrameOnly.HasValue && IFrameOnly.Value)
            {
                strBuilder.AppendLine(Tag.EXTXIFRAMESONLY);
            }

            if (Map != null)
            {
                strBuilder.AppendLine(Map.ToString());
            }

            foreach (var segment in MediaSegments)
            {
                strBuilder.AppendLine();
                strBuilder.Append(segment);
            }

            if (HasEndList)
            {
                strBuilder.AppendLine(Tag.EXTXENDLIST);
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}