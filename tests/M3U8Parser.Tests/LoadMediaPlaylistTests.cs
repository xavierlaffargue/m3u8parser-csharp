using NUnit.Framework;

namespace M3U8Parser.Tests
{
    using M3U8Parser.CustomType;
    using M3U8Parser.ExtXType;
    using System;
	using System.IO;

	public class LoadMediaPlaylistTests
	{
		private MediaPlaylist _mediaPlaylist = new ();

		[SetUp]
		public void Setup()
		{
			//_mediaPlaylist = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_1.m3u8");
		}
		
		[Test]
		public void ShouldBePlaylistTypeVod()
		{
			var mediaPlaylist = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_1.m3u8");
			
			Assert.AreEqual(PlaylistType.Vod, mediaPlaylist.PlaylistType);
		}
		
	}
}