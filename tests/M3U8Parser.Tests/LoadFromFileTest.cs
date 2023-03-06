using NUnit.Framework;

namespace M3U8Parser.Tests
{
	using System.IO;

	public class LoadFromFileTest
	{
		[Test]
		public void LoadFromFileMediaPlaylist()
		{
			var result = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_1.m3u8");
			
			Assert.NotNull(result);
		}
		
		[Test]
		public void LoadFromFileMasterPlaylist()
		{
			var result = MasterPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "manifest_1.m3u8");
			
			Assert.NotNull(result);
		}
	}
}