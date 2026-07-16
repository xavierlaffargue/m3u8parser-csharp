namespace M3U8Parser
{
    using System;
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

        public ServerControl ServerControl { get; set; }

        public Skip Skip { get; set; }

        public PartInf PartInf { get; set; }

        public List<PreloadHint> PreloadHints { get; set; } = new ();

        public List<RenditionReport> RenditionReports { get; set; } = new ();

        public List<Part> Parts { get; set; } = new ();

        public List<Define> Defines { get; set; } = new ();

        public List<DateRange> DateRanges { get; set; } = new ();

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
            IndependentSegments independentSegments = null;
            List<MediaSegment> mediaSegments = new ();
            var hlsVersion = 4;
            var hasEndList = false;
            int? targetDuration = null;
            int? mediaSequence = null;
            bool? iFrameOnly = null;

            ServerControl serverControl = null;
            Skip skip = null;
            PartInf partInf = null;
            List<PreloadHint> preloadHints = new ();
            List<RenditionReport> renditionReports = new ();
            List<Define> defines = new ();
            List<DateRange> dateRanges = new ();

            Map currentMap = null;
            string currentProgramDateTime = null;
            Key currentKey = null;
            bool currentGap = false;
            int? currentBitrate = null;
            List<Part> currentParts = new ();

            using (var reader = new StringReader(text))
            {
                List<Segment> currentSegments = new ();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line) || line == Tag.EXTM3U)
                    {
                        continue;
                    }

                    if (line.StartsWith(Tag.EXTXVERSION))
                    {
                        hlsVersion = new HlsVersion(line).Value;
                    }
                    else if (line.StartsWith(Tag.EXTXPLAYLISTTYPE))
                    {
                        playlistType = new PlaylistTypeExt(line).Value;
                    }
                    else if (line.StartsWith(Tag.EXTXINDEPENDENTSEGMENTS))
                    {
                        independentSegments = new IndependentSegments(line);
                    }
                    else if (line.StartsWith(Tag.EXTXTARGETDURATION))
                    {
                        targetDuration = new TargetDuration(line).Value;
                    }
                    else if (line.StartsWith(Tag.EXTXMEDIASEQUENCE))
                    {
                        mediaSequence = new MediaSequence(line).Value;
                    }
                    else if (line.StartsWith(Tag.EXTXIFRAMESONLY))
                    {
                        iFrameOnly = true;
                    }
                    else if (line.StartsWith(Tag.EXTXENDLIST))
                    {
                        hasEndList = true;
                    }
                    else if (line.StartsWith(Tag.EXTXMAP))
                    {
                        currentMap = new Map(line);
                    }
                    else if (line.StartsWith(Tag.EXTXPROGRAMDATETIME))
                    {
                        var match = Regex.Match(line, $"(?<={Tag.EXTXPROGRAMDATETIME}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        currentProgramDateTime = match.Success ? match.Groups[0].Value : line.Split(':')[1].Trim();
                    }
                    else if (line.StartsWith(Tag.EXTXKEY))
                    {
                        if (currentSegments.Count > 0 || (currentKey != null && currentKey.Method != MethodType.None))
                        {
                            mediaSegments.Add(new MediaSegment { Key = currentKey, Segments = currentSegments });
                            currentSegments = new List<Segment>();
                        }

                        currentKey = new Key(line);
                    }
                    else if (line.StartsWith(Tag.EXTXSERVERCONTROL))
                    {
                        serverControl = new ServerControl(line);
                    }
                    else if (line.StartsWith(Tag.EXTXSKIP))
                    {
                        skip = new Skip(line);
                    }
                    else if (line.StartsWith(Tag.EXTXPARTINF))
                    {
                        partInf = new PartInf(line);
                    }
                    else if (line.StartsWith(Tag.EXTXPRELOADHINT))
                    {
                        preloadHints.Add(new PreloadHint(line));
                    }
                    else if (line.StartsWith(Tag.EXTXRENDITIONREPORT))
                    {
                        renditionReports.Add(new RenditionReport(line));
                    }
                    else if (line.StartsWith(Tag.EXTXDEFINE))
                    {
                        defines.Add(new Define(line));
                    }
                    else if (line.StartsWith(Tag.EXTXDATERANGE))
                    {
                        dateRanges.Add(new DateRange(line));
                    }
                    else if (line.StartsWith(Tag.EXTXGAP))
                    {
                        currentGap = true;
                    }
                    else if (line.StartsWith(Tag.EXTXBITRATE))
                    {
                        var match = Regex.Match(line, $"(?<={Tag.EXTXBITRATE}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            currentBitrate = int.Parse(match.Groups[0].Value);
                        }
                    }
                    else if (line.StartsWith(Tag.EXTXPART))
                    {
                        currentParts.Add(new Part(line));
                    }
                    else if (line.StartsWith(Tag.EXTINF))
                    {
                        var segmentBlock = new StringBuilder();
                        segmentBlock.AppendLine(line);

                        string nextLine;
                        while ((nextLine = reader.ReadLine()) != null)
                        {
                            string trimmedNext = nextLine.Trim();
                            if (string.IsNullOrEmpty(trimmedNext))
                            {
                                continue;
                            }

                            segmentBlock.AppendLine(trimmedNext);
                            if (!trimmedNext.StartsWith("#"))
                            {
                                break;
                            }
                        }

                        var segment = new Segment(segmentBlock.ToString());
                        segment.Map = currentMap;
                        segment.ProgramDateTime = currentProgramDateTime;
                        segment.Gap = currentGap;
                        segment.Bitrate = currentBitrate;
                        segment.Parts = currentParts;

                        currentProgramDateTime = null; // Reset as per RFC 8216
                        currentGap = false; // Reset for next segment
                        currentParts = new List<Part>(); // Reset for next segment

                        currentSegments.Add(segment);
                    }
                }

                if (currentSegments.Count > 0 || (currentKey != null && currentKey.Method != MethodType.None))
                {
                    mediaSegments.Add(new MediaSegment { Key = currentKey, Segments = currentSegments });
                }
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
                IndependentSegments = independentSegments,
                ServerControl = serverControl,
                Skip = skip,
                PartInf = partInf,
                PreloadHints = preloadHints,
                RenditionReports = renditionReports,
                Parts = currentParts,
                Defines = defines,
                DateRanges = dateRanges
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

            if (Defines is { Count: > 0 })
            {
                foreach (var define in Defines)
                {
                    strBuilder.AppendLine(define.ToString());
                }
            }

            if (ServerControl != null)
            {
                strBuilder.AppendLine(ServerControl.ToString());
            }

            if (PartInf != null)
            {
                strBuilder.AppendLine(PartInf.ToString());
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

            if (Skip != null)
            {
                strBuilder.AppendLine(Skip.ToString());
            }

            if (DateRanges is { Count: > 0 })
            {
                foreach (var dateRange in DateRanges)
                {
                    strBuilder.AppendLine(dateRange.ToString());
                }
            }

            foreach (var segment in MediaSegments)
            {
                strBuilder.AppendLine();
                strBuilder.Append(segment);
            }

            if (Parts is { Count: > 0 })
            {
                strBuilder.AppendLine();
                foreach (var part in Parts)
                {
                    strBuilder.AppendLine(part.ToString());
                }
            }

            if (PreloadHints is { Count: > 0 })
            {
                foreach (var hint in PreloadHints)
                {
                    strBuilder.AppendLine(hint.ToString());
                }
            }

            if (RenditionReports is { Count: > 0 })
            {
                foreach (var report in RenditionReports)
                {
                    strBuilder.AppendLine(report.ToString());
                }
            }

            if (HasEndList)
            {
                strBuilder.AppendLine(Tag.EXTXENDLIST);
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}
