namespace M3U8Parser.ExtXType
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using M3U8Parser.Attributes.BaseAttribute;
    using M3U8Parser.Interfaces;

    public class MediaSegment : IExtXType
    {
        public MediaSegment()
        {
        }

        public MediaSegment(string str)
        {
            using var reader = new StringReader(str);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(Key.Prefix))
                {
                    Key = new Key(line);
                }
                else if (line.StartsWith(Segment.Prefix))
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

            if (Segments != null && Segments.Count > 0)
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