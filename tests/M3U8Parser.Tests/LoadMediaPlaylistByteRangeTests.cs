using System;
using System.Collections.Generic;
using System.IO;
using M3U8Parser.Attributes.BaseAttribute;
using M3U8Parser.ExtXType;
using Xunit;

namespace M3U8Parser.Tests;

public class LoadMediaPlaylistByteRangeTests
{
    private readonly MediaPlaylist _mediaPlaylist;

    public LoadMediaPlaylistByteRangeTests()
    {
        _mediaPlaylist =
            MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_2.m3u8");
    }

    [Fact]
    public void PlaylistTypeShouldBePVod()
    {
        Assert.Equal(PlaylistType.Vod, _mediaPlaylist.PlaylistType);
    }

    [Fact]
    public void TargetDurationShouldBe10()
    {
        Assert.Equal(11, _mediaPlaylist.TargetDuration);
    }

    [Fact]
    public void MediaSequenceShouldBe0()
    {
        Assert.Equal(1, _mediaPlaylist.MediaSequence);
    }

    [Fact]
    public void MediaSequenceShouldBeTrue()
    {
        Assert.Null(_mediaPlaylist.IFrameOnly);
    }

    [Fact]
    public void HlsVersionShouldBe4()
    {
        Assert.Equal(4, _mediaPlaylist.HlsVersion);
    }

    [Fact]
    public void MediaSegmentKeyShouldBeNull()
    {
        Assert.Null(_mediaPlaylist.MediaSegments[0].Key);
    }

    [Fact]
    public void MediaSegmentCountShouldBe4()
    {
        Assert.Equal(296, _mediaPlaylist.MediaSegments[0].Segments.Count);
    }

    [Fact]
    public void FirstMediaSegmentDurationShouldBe10()
    {
        Assert.Equal(10.005, _mediaPlaylist.MediaSegments[0].Segments[0].Duration);
    }

    [Fact]
    public void UrlMediaSegmentDurationShouldBefileSequenceTs()
    {
        Assert.Equal("mediaFileSample.aac", _mediaPlaylist.MediaSegments[0].Segments[0].Uri);
    }

    [Fact]
    public void WriteToString()
    {
        var mediaPlaylist =
            MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_3.m3u8");
        Console.WriteLine(mediaPlaylist.ToString());
    }

    [Fact]
    public void CreateMediaPlaylist()
    {
        var mediaPlaylist = new MediaPlaylist
        {
            HasEndList = true,
            HlsVersion = 7,
            IFrameOnly = true,
            PlaylistType = PlaylistType.Vod,
            TargetDuration = 3600,
            MediaSequence = null,
            MediaSegments =
            {
                new MediaSegment
                {
                    Key = new Key
                    {
                        Method = MethodType.AES_128, Uri = "http://test.local"
                    },
                    Segments = new List<Segment>
                    {
                        new()
                        {
                            ByteRangeLentgh = 100,
                            ByteRangeStartSubRange = 0,
                            Duration = 100,
                            Uri = "http://test/1.mp4"
                        },
                        new()
                        {
                            ByteRangeLentgh = 100,
                            ByteRangeStartSubRange = 100,
                            Duration = 100,
                            Uri = "http://test/2.mp4"
                        }
                    }
                },
                new MediaSegment
                {
                    Key = new Key
                    {
                        Method = MethodType.AES_128, Uri = "http://test2.local"
                    },
                    Segments = new List<Segment>
                    {
                        new()
                        {
                            ByteRangeLentgh = 100,
                            ByteRangeStartSubRange = 300,
                            Duration = 100,
                            Uri = "http://test/3.mp4"
                        },
                        new()
                        {
                            ByteRangeLentgh = 100,
                            ByteRangeStartSubRange = 400,
                            Duration = 100,
                            Uri = "http://test/4.mp4"
                        }
                    }
                }
            }
        };

        Console.WriteLine(mediaPlaylist.ToString());
    }
}