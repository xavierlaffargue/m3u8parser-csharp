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
			var mediaPlaylist = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_2.m3u8");
			Assert.AreEqual(PlaylistType.Vod, mediaPlaylist.PlaylistType);
		}
		
		[Test]
		public void ShouldBeHlsVersion4()
		{
			var mediaPlaylist = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_2.m3u8");
			Assert.AreEqual(4, mediaPlaylist.HlsVersion);
		}

		[Test]
		public void WriteToString()
		{
			var mediaPlaylist = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_3.m3u8");
			Console.WriteLine(mediaPlaylist.ToString());
		}
	}
}