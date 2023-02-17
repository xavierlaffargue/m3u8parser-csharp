namespace M3U8Parser
{
	using M3U8Parser.ExtXType;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class MediaSegment : IExtXType
	{
		public double Duration { get; set; }
		
		public string Title { get; set; }
		
		public string Uri { get; set; }
		
		public long? ByteRangeLentgh { get; set; }
		
		public long? ByteRangeStartSubRange { get; set; }
		
		public MediaSegment()
		{
		}

		public MediaSegment(string str)
		{
			using var reader = new StringReader(str);
			var line = "";
			while ((line = reader.ReadLine()) != null)
			{
				if (line.StartsWith("#EXTINF"))
				{
					var match = Regex.Match(line.Trim(), $"(?<=#EXTINF:)(.*?)(?=$)",
						RegexOptions.Multiline & RegexOptions.IgnoreCase);

					if (match.Success)
					{
						Duration = double.Parse(match.Groups[0].Value.Split(',')[0]);
					}
				}
				else if (line.StartsWith("#EXT-X-BYTERANGE"))
				{
					var match = Regex.Match(line.Trim(), $"(?<=#EXT-X-BYTERANGE:)(.*?)(?=$)",
						RegexOptions.Multiline & RegexOptions.IgnoreCase);

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
				else if (!line.StartsWith("#EXT"))
				{
					Uri = line;
				}
			}
		}

		public static string Prefix => "#EXTINF";

		public override string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.AppendLine($"{Prefix}:{Duration.ToString()},{Title}");

			if (ByteRangeLentgh != null)
			{
				strBuilder.Append($"#EXT-X-BYTERANGE:{ByteRangeLentgh}");

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