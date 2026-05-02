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

        public IndependentSegments IndependentSegments { get; set; } = new ();

        public Map Map
        {
            get
            {
                if (MediaSegments.Count > 0)
                {
                    foreach (var mediaSegment in MediaSegments)
                    {
                        if (mediaSegment.Segments != null)
                        {
                            foreach (var segment in mediaSegment.Segments)
                            {
                                if (segment.Map != null)
                                {
                                    return segment.Map;
                                }
                            }
                        }
                    }
                }

                return null;
            }

            set
            {
                if (MediaSegments.Count > 0 && MediaSegments[0].Segments.Count > 0)
                {
                    MediaSegments[0].Segments[0].Map = value;
                }
            }
        }

        public int HlsVersion { get; set; }

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
            IndependentSegments IndependentSegments = null;
            List<MediaSegment> mediaSegments = new ();
            var hlsVersion = 4;
            var hasEndList = false;
            int? targetDuration = null;
            int? mediaSequence = null;
            bool? iFrameOnly = null;

            var regex = new Regex($"(?={Tag.EXTX})(.*?)(?<=$)", RegexOptions.Multiline);
            var matches = regex.Matches(text);

            Map globalPlaylistMap = null;
            foreach (Match match in matches)
            {
                var line = match.Value;
                if (line.StartsWith(Tag.EXTXPLAYLISTTYPE))
                {
                    playlistType = new PlaylistTypeExt(line).Value;
                }
                else if (line.StartsWith(Tag.EXTXVERSION))
                {
                    hlsVersion = new HlsVersion(line).Value;
                }
                else if (line.StartsWith(Tag.EXTXINDEPENDENTSEGMENTS))
                {
                    IndependentSegments = new IndependentSegments(line);
                }
                else if (line.StartsWith(Tag.EXTXMAP))
                {
                    globalPlaylistMap = new Map(line);
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

            var l = Regex.Split(text, $"(?={Tag.EXTXKEY}|#EXTINF:|{Tag.EXTXMAP}|{Tag.EXTXPROGRAMDATETIME})");
            var segments = new List<Segment>();
            MediaSegment mediaSegment = null;
            string currentProgramDateTime = null;

            foreach (var line in l)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#EXTM3U"))
                {
                    continue;
                }

                if (line.StartsWith(Tag.EXTXMAP))
                {
                    globalPlaylistMap = new Map(line);
                    continue;
                }

                if (line.StartsWith(Tag.EXTXPROGRAMDATETIME))
                {
                    var match = Regex.Match(line.Trim(), $"(?<={Tag.EXTXPROGRAMDATETIME}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        currentProgramDateTime = match.Groups[0].Value;
                    }
                    else
                    {
                        // Fallback: try to extract it from the line directly if the above fails
                        var parts = line.Split(':');
                        if (parts.Length > 1)
                        {
                            currentProgramDateTime = string.Join(":", parts, 1, parts.Length - 1).Trim();
                        }
                    }
                    continue;
                }

                if (line.StartsWith(Tag.EXTXKEY))
                {
                    if (mediaSegment != null)
                    {
                        mediaSegment.Segments = segments;
                        mediaSegments.Add(mediaSegment);
                    }

                    mediaSegment = new MediaSegment(line);
                    segments = new List<Segment>();
                }
                else if (line.StartsWith(Tag.EXTINF))
                {
                    mediaSegment ??= new MediaSegment();

                    var segment = new Segment(line);
                    if (segment.Map != null)
                    {
                        globalPlaylistMap = segment.Map;
                    }

                    if (segment.Map == null && globalPlaylistMap != null)
                    {
                        segment.Map = globalPlaylistMap;
                    }

                    if (segment.ProgramDateTime == null && currentProgramDateTime != null)
                    {
                        segment.ProgramDateTime = currentProgramDateTime;
                        currentProgramDateTime = null;
                    }

                    segments.Add(segment);
                }
            }

            if (mediaSegment != null)
            {
                mediaSegment.Segments = segments;
                mediaSegments.Add(mediaSegment);
            }

            return new MediaPlaylist
            {
                PlaylistType = playlistType,
                HlsVersion = hlsVersion,
                MediaSegments = mediaSegments,
                HasEndList = hasEndList,
                TargetDuration = targetDuration,
                MediaSequence = mediaSequence,
                IFrameOnly = iFrameOnly,
                IndependentSegments = IndependentSegments
            };
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine(Tag.EXTM3U);
            strBuilder.AppendLine($"{Tag.EXTXVERSION}:{HlsVersion}");

            if (IndependentSegments != null && IndependentSegments.IsPresent)
            {
                strBuilder.AppendLine(IndependentSegments.ToString());
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