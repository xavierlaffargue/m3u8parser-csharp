using System;
using System.Collections.Generic;
using System.IO;
using M3U8Parser.Attributes.ValueType;
using M3U8Parser.Tags.MediaSegment;
using Xunit;

namespace M3U8Parser.Tests;

public class LoadMediaPlaylistTests
{
    private readonly MediaPlaylist _mediaPlaylist;

    public LoadMediaPlaylistTests()
    {
        _mediaPlaylist =
            MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_1.m3u8");
    }

    [Fact]
    public void PlaylistTypeShouldBePVod()
    {
        Assert.Equal(PlaylistType.Vod, _mediaPlaylist.PlaylistType);
    }

    [Fact]
    public void TargetDurationShouldBe10()
    {
        Assert.Equal(10, _mediaPlaylist.TargetDuration);
    }

    [Fact]
    public void MediaSequenceShouldBe0()
    {
        Assert.Equal(0, _mediaPlaylist.MediaSequence);
    }

    [Fact]
    public void MediaSequenceShouldBeTrue()
    {
        Assert.True(_mediaPlaylist.IFrameOnly);
    }

    [Fact]
    public void IndependentSegmentIsPresent()
    {
        Assert.True(_mediaPlaylist.IndependentSegments?.IsPresent);
    }
    
    [Fact]
    public void HlsVersionShouldBe4()
    {
        Assert.Equal(4, _mediaPlaylist.HlsVersion);
    }

    [Fact]
    public void MapShouldExist()
    {
        Assert.NotNull(_mediaPlaylist.Map);
    }

    [Fact]
    public void MapUriShouldBeCorrect()
    {
        Assert.Equal("main.mp4", _mediaPlaylist.Map.Uri);
    }

    [Fact]
    public void MediaSegmentKeyShouldBeNull()
    {
        Assert.Null(_mediaPlaylist.MediaSegments[0].Key);
    }

    [Fact]
    public void MediaSegmentCountShouldBe4()
    {
        Assert.Equal(4, _mediaPlaylist.MediaSegments[0].Segments.Count);
    }

    [Fact]
    public void FirstMediaSegmentDurationShouldBe10()
    {
        Assert.Equal(10, _mediaPlaylist.MediaSegments[0].Segments[0].Duration);
    }

    [Fact]
    public void UrlMediaSegmentDurationShouldBefileSequenceTs()
    {
        Assert.Equal("http://example.com/movie1/fileSequenceA.ts", _mediaPlaylist.MediaSegments[0].Segments[0].Uri);
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