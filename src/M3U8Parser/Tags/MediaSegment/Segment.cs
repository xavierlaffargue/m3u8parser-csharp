namespace M3U8Parser.Tags.MediaSegment
{
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.Interfaces;

    public class Segment : ITag
    {
        public Segment()
        {
        }

        public Segment(string str)
        {
            using var reader = new StringReader(str);
            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith(Tag.EXTINF))
                {
                    var match = Regex.Match(line.Trim(), $"(?<={Tag.EXTINF}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        var durationStr = match.Groups[0].Value.Split(',')[0];
                        Duration = double.Parse(durationStr, CultureInfo.InvariantCulture);
                    }
                }
                else if (line.StartsWith(Tag.EXTXBYTERANGE))
                {
                    var match = Regex.Match(line.Trim(), $"(?<={Tag.EXTXBYTERANGE}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        var byterange = match.Groups[0].Value.Split('@');
                        ByteRangeLentgh = long.Parse(byterange[0]);

                        if (byterange.Length > 1)
                        {
                            ByteRangeStartSubRange = long.Parse(byterange[1]);
                        }
                    }
                }
                else if (line.StartsWith(Tag.EXTXPROGRAMDATETIME))
                {
                    var match = Regex.Match(line.Trim(), $"(?<={Tag.EXTXPROGRAMDATETIME}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        ProgramDateTime = match.Groups[0].Value;
                    }
                }
                else if (line.StartsWith(Tag.EXTXMAP))
                {
                    Map = new Map(line);
                }
                else if (!line.StartsWith("#EXT"))
                {
                    Uri = line;
                }
            }
        }

        public double Duration { get; set; }

        public string Title { get; set; }

        public string Uri { get; set; }

        public long? ByteRangeLentgh { get; set; }

        public long? ByteRangeStartSubRange { get; set; }

        public string ProgramDateTime { get; set; }

        public Map Map { get; set; }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            if (ProgramDateTime != null)
            {
                strBuilder.AppendLine($"{Tag.EXTXPROGRAMDATETIME}:{ProgramDateTime}");
            }

            if (Map != null)
            {
                strBuilder.AppendLine(Map.ToString());
            }

            strBuilder.AppendLine($"{Tag.EXTINF}:{Duration.ToString(CultureInfo.InvariantCulture)},{Title}");

            if (ByteRangeLentgh != null)
            {
                strBuilder.Append($"{Tag.EXTXBYTERANGE}:{ByteRangeLentgh}");

                if (ByteRangeStartSubRange != null)
                {
                    strBuilder.Append($"@{ByteRangeStartSubRange}");
                }

                strBuilder.AppendLine();
            }

            strBuilder.AppendLine(Uri);

            return strBuilder.ToString();
        }
    }
}