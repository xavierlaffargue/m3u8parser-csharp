using System;
using System.Collections.Generic;
using System.IO;
using M3U8Parser.Attributes.BaseAttribute;
using M3U8Parser.ExtXType;
using NUnit.Framework;

namespace M3U8Parser.Tests;

public class LoadMediaPlaylistByteRangeTests
{
    private MediaPlaylist _mediaPlaylist = new();

    [SetUp]
    public void Setup()
    {
        _mediaPlaylist =
            MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_2.m3u8");
    }

    [Test]
    public void PlaylistTypeShouldBePVod()
    {
        Assert.AreEqual(PlaylistType.Vod, _mediaPlaylist.PlaylistType);
    }

    [Test]
    public void TargetDurationShouldBe10()
    {
        Assert.AreEqual(11, _mediaPlaylist.TargetDuration);
    }

    [Test]
    public void MediaSequenceShouldBe0()
    {
        Assert.AreEqual(1, _mediaPlaylist.MediaSequence);
    }

    [Test]
    public void MediaSequenceShouldBeTrue()
    {
        Assert.AreEqual(null, _mediaPlaylist.IFrameOnly);
    }

    [Test]
    public void HlsVersionShouldBe4()
    {
        Assert.AreEqual(4, _mediaPlaylist.HlsVersion);
    }

    [Test]
    public void MediaSegmentKeyShouldBeNull()
    {
        Assert.AreEqual(null, _mediaPlaylist.MediaSegments[0].Key);
    }

    [Test]
    public void MediaSegmentCountShouldBe4()
    {
        Assert.AreEqual(296, _mediaPlaylist.MediaSegments[0].Segments.Count);
    }

    [Test]
    public void FirstMediaSegmentDurationShouldBe10()
    {
        Assert.AreEqual(10.005, _mediaPlaylist.MediaSegments[0].Segments[0].Duration);
    }

    [Test]
    public void UrlMediaSegmentDurationShouldBefileSequenceTs()
    {
        Assert.AreEqual("mediaFileSample.aac", _mediaPlaylist.MediaSegments[0].Segments[0].Uri);
    }

    [Test]
    public void WriteToString()
    {
        var mediaPlaylist =
            MediaPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "mediaplaylist_vod_3.m3u8");
        Console.WriteLine(mediaPlaylist.ToString());
    }

    [Test]
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