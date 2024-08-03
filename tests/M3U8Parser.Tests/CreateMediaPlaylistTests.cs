using System;
using Xunit;

namespace M3U8Parser.Tests;

public class CreateMediaPlaylistTests
{
    [Fact]
    public void DefaultHlsValueShouldContainOnlyVersion4()
    {
        var mp = new MasterPlaylist();
        Assert.Equal(4, mp.HlsVersion);
        Assert.Equal($"#EXTM3U{Environment.NewLine}#EXT-X-VERSION:4", mp.ToString());
    }
}