using System.IO;
using Xunit;

namespace M3U8Parser.Tests;

public class LoadFromFileTest
{
    [Fact]
    public void LoadFromFileMediaPlaylist()
    {
        var result = MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_1.m3u8");

        Assert.NotNull(result);
    }

    [Fact]
    public void LoadFromFileMasterPlaylist()
    {
        var result = MasterPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "manifest_1.m3u8");

        Assert.NotNull(result);
    }
}