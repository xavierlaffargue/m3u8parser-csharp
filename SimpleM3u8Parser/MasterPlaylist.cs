namespace SimpleM3u8Parser
{
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	public class MasterPlaylist
	{
		public List<Media> Medias = new ();
		public List<IframeStreamInf> IFrameStreams = new ();
		public List<StreamInf> Streams = new ();

		public MasterPlaylist()
		{}

		public MasterPlaylist(string text)
		{
			var l = Regex.Split(text, "(?=#EXT-X)");

            foreach (var line in l)
			{
				if(line.StartsWith(Media.Prefix))
				{
					Media media = new Media(line);
					Medias.Add(media);
				}
				
				if(line.StartsWith(IframeStreamInf.Prefix))
				{
					IframeStreamInf streaminf = new IframeStreamInf(line);
					IFrameStreams.Add(streaminf);
				}
				
				if(line.StartsWith(StreamInf.Prefix))
				{
					StreamInf stream = new StreamInf(line);
					Streams.Add(stream);
				}
			}
        }

	}
}