namespace M3U8Parser
{
	using M3U8Parser.ExtXType;

	public class MediaSegment : BaseExtX
	{
		public static string Prefix = "#EXTINF";
		
		protected override string ExtPrefix => MediaSegment.Prefix;
		
		public MediaSegment()
		{
			
		}
		
		public MediaSegment(string str)
		{
			
		}
	}
}