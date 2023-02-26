namespace M3U8Parser
{
	using M3U8Parser.ExtXType;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class MediaSegment : IExtXType
	{
		public List<Segment> Segments { get; set; } = new List<Segment>();
		
		public Key Key { get; set; }
		
		
		public MediaSegment()
		{
		}

		public MediaSegment(string str)
		{
			using var reader = new StringReader(str);
			var line = "";
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

		public override string ToString()
		{
			var strBuilder = new StringBuilder();

			if(Key != null && Key.Method != CustomType.MethodType.None)
			{
                strBuilder.AppendLine(Key.ToString());
            }
			if(Segments != null && Segments.Count > 0)
			{
				foreach (var segment in Segments)
				{
                    strBuilder.Append(segment.ToString());
                }
			}

			return strBuilder.ToString();
		}
	}
}