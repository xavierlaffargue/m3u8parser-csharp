using System;
using NUnit.Framework;

namespace M3U8Parser.Tests;

public class CreateMediaPlaylistTests
{
    [Test]
    public void DefaultHlsValueShouldContainOnlyVersion4()
    {
        var mp = new MasterPlaylist();
        Assert.AreEqual(4, mp.HlsVersion);
        Assert.AreEqual($"#EXTM3U{Environment.NewLine}#EXT-X-VERSION:4", mp.ToString());
    }
}