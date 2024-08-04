namespace M3U8Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using M3U8Parser.Attributes.ValueType;
    using M3U8Parser.Interfaces;
    using M3U8Parser.Tags.MediaSegment;

    public class MediaSegment : ITag
    {
        public MediaSegment()
        {
        }

        public MediaSegment(string str)
        {
            using var reader = new StringReader(str);
            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith(Tag.EXTXKEY))
                {
                    Key = new Key(line);
                }
                else if (line.StartsWith(Tag.EXTINF))
                {
                    var segment = new Segment(line);
                    Segments.Add(segment);
                }
            }
        }

        public List<Segment> Segments { get; set; } = new ();

        public Key Key { get; set; }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            if (Key != null && Key.Method != MethodType.None)
            {
                strBuilder.AppendLine(Key.ToString());
            }

            if (Segments is { Count: > 0 })
            {
                foreach (var segment in Segments)
                {
                    strBuilder.Append(segment);
                }
            }

            return strBuilder.ToString();
        }
    }
}