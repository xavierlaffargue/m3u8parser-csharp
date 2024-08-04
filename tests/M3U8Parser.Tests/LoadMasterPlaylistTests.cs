using System;
using System.IO;
using M3U8Parser.Attributes.ValueType;
using M3U8Parser.Tags.MultivariantPlaylist;
using Xunit;

namespace M3U8Parser.Tests;

public class LoadMasterPlaylistTests
{
    private readonly MasterPlaylist _masterPlaylist;

    public LoadMasterPlaylistTests()
    {
        _masterPlaylist = MasterPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "manifest_1.m3u8");
    }

    [Fact]
    public void ShouldBeHave5Medias()
    {
        Assert.Equal(5, _masterPlaylist.Medias.Count);
    }

    [Fact]
    public void ShouldBeHave5Streams()
    {
        Assert.Equal(7, _masterPlaylist.Streams.Count);
    }

    [Fact]
    public void ShouldBeHave5IFrameStreams()
    {
        Assert.Equal(6, _masterPlaylist.IFrameStreams.Count);
    }

    [Fact]
    public void ShouldBeHlsVersion7()
    {
        Assert.Equal(7, _masterPlaylist.HlsVersion);
    }

    [Fact]
    public void CheckFirstMedia()
    {
        Assert.Equal(MediaType.Audio, _masterPlaylist.Medias[0].Type);
        Assert.Equal("audio", _masterPlaylist.Medias[0].GroupId);
        Assert.Equal("English", _masterPlaylist.Medias[0].Name);
        Assert.Equal("eng", _masterPlaylist.Medias[0].Language);
        Assert.True(_masterPlaylist.Medias[0].AutoSelect);
        Assert.True(_masterPlaylist.Medias[0].Default);
        Assert.Equal("QualityLevels(192000)/Manifest(audio_eng_aacl,format=m3u8-aapl,filter=desktop)",
            _masterPlaylist.Medias[0].Uri);
    }

    [Fact]
    public void AfterEditMediaTypeShouldBeChanged()
    {
        _masterPlaylist.Medias[0].Type = MediaType.Video;
        Assert.Equal(MediaType.Video, _masterPlaylist.Medias[0].Type);
    }

    [Fact]
    public void AfterEditAutoSelectShouldBeChanged()
    {
        _masterPlaylist.Medias[0].AutoSelect = false;
        Assert.False(_masterPlaylist.Medias[0].AutoSelect);
    }

    [Fact]
    public void AfterEditLanguageShouldBeChanged()
    {
        _masterPlaylist.Medias[0].Language = "fre";
        Assert.Equal("fre", _masterPlaylist.Medias[0].Language);
    }

    [Fact]
    public void DefaultVersionShouldBe4()
    {
        var masterPlaylist = new MasterPlaylist();
        Assert.Equal(4, masterPlaylist.HlsVersion);
    }

    [Fact]
    public void ParseClosedCaptionsMediaShouldBeOk()
    {
        var media = new Media(
            "#EXT-X-MEDIA:TYPE=CLOSED-CAPTIONS,GROUP-ID=\"cc\",NAME=\"CEA608_CC\",AUTOSELECT=NO,DEFAULT=NO,INSTREAM-ID=\"CC1\"");
        Assert.Equal(MediaType.CloseCaptions, media.Type);
        Assert.Equal("cc", media.GroupId);
        Assert.Equal("CEA608_CC", media.Name);
        Assert.False(media.AutoSelect);
        Assert.False(media.Default);
        Assert.Equal("CC1", media.InstreamId);
        Assert.Null(media.Language);
    }

    [Fact]
    public void ParseVideoMediaShouldBeOk()
    {
        var media = new Media(
            "#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
        Assert.Equal(MediaType.Audio, media.Type);
        Assert.Equal("audio", media.GroupId);
        Assert.Equal("English", media.Name);
        Assert.Equal("eng", media.Language);
        Assert.True(media.AutoSelect);
        Assert.True(media.Default);
        Assert.Equal("uri/Manifest", media.Uri);
        Assert.Null(media.InstreamId);
    }

    [Fact]
    public void ParseStreamInfShouldBeOk()
    {
        var media = new StreamInf(
            "#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=2778321,BANDWIDTH=3971374,VIDEO-RANGE=PQ,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-1\r\nsdr_720/prog_index.m3u8");
        Assert.Equal(2778321, media.AverageBandwidth);
        Assert.Equal(3971374, media.Bandwidth);
        Assert.Equal(VideoRangeType.PQ, media.VideoRange);
        Assert.Equal("hvc1.2.4.L123.B0", media.Codecs);
        Assert.Equivalent(23.976, media.FrameRate);
        Assert.Equal(HdcpLevelType.TYPE_1, media.HdcpLevel);
        Assert.Equal(1280, media.Resolution.Width);
        Assert.Equal(720, media.Resolution.Height);
        Assert.Equal("sdr_720/prog_index.m3u8", media.Uri);
    }

    [Fact]
    public void EditResolutionShouldBeOk()
    {
        var media = new StreamInf("#EXT-X-STREAM-INF:RESOLUTION=1280x720")
        {
            Resolution =
            {
                Width = 1920,
                Height = 1080
            }
        };

        Assert.Equal(1920, media.Resolution.Width);
        Assert.Equal(1080, media.Resolution.Height);
    }

    [Fact]
    public void ParseAndToStringMediaShouldBeEqual()
    {
        var media = new Media(
            "#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
        Assert.Equal(
            "#EXT-X-MEDIA:AUTOSELECT=YES,DEFAULT=YES,GROUP-ID=\"audio\",LANGUAGE=\"eng\",TYPE=AUDIO,NAME=\"English\",URI=\"uri/Manifest\"",
            media.ToString());
    }

    [Fact]
    public void ParseAndToStringMediaShouldBeEqualWithDoubleQuote()
    {
        var media = new Media(
            "#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English \\\"Original\\\"\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
        Assert.Equal(
            "#EXT-X-MEDIA:AUTOSELECT=YES,DEFAULT=YES,GROUP-ID=\"audio\",LANGUAGE=\"eng\",TYPE=AUDIO,NAME=\"English \\\"Original\\\"\",URI=\"uri/Manifest\"",
            media.ToString());
    }

    [Fact]
    public void ParseAndToStringStreamShouldBeEqual()
    {
        var media = new StreamInf(
            "#EXT-X-STREAM-INF:BANDWIDTH=3971374,AVERAGE-BANDWIDTH=2778321,VIDEO-RANGE=SDR,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,HDCP-LEVEL=NONE,SUBTITLES=\"subtitle\"\r\nsdr_720/prog_index.m3u8");
        Assert.Equal(
            "#EXT-X-STREAM-INF:BANDWIDTH=3971374,AVERAGE-BANDWIDTH=2778321,VIDEO-RANGE=SDR,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,HDCP-LEVEL=NONE,SUBTITLES=\"subtitle\"\r\nsdr_720/prog_index.m3u8",
            media.ToString());
    }

    [Fact]
    public void OptionnalAttributesShouldBeNullable()
    {
        var media = new StreamInf(
            "#EXT-X-STREAM-INF:BANDWIDTH=2778321,CODECS=\"hvc1.2.4.L123.B0\"\r\nsdr_720/prog_index.m3u8");
        Assert.Null(media.AverageBandwidth);
        Assert.Null(media.FrameRate);
        Assert.Null(media.VideoRange);
        Assert.Null(media.Resolution);
        Assert.Null(media.HdcpLevel);
    }

    [Fact]
    public void ParseAndToStringIFrameStreamShouldBeEqual()
    {
        var media = new IframeStreamInf(
            "#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=621335,CODECS=\"avc1.42c00d\",RESOLUTION=416x234,URI=\"QualityLevels(399992)/Manifest(video,format=m3u8-aapl,filter=desktop,type=keyframes)\"");
        Assert.Equal(
            "#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=621335,CODECS=\"avc1.42c00d\",RESOLUTION=416x234,URI=\"QualityLevels(399992)/Manifest(video,format=m3u8-aapl,filter=desktop,type=keyframes)\"",
            media.ToString());
    }

    [Fact]
    public void MasterPlaylistShouldBeContainExtM3U()
    {
        Assert.Contains("#EXTM3U" + Environment.NewLine, _masterPlaylist.ToString());
    }

    [Fact]
    public void MasterPlaylistShouldBeContainVersionExtM3U()
    {
        Assert.Contains("#EXT-X-VERSION:7" + Environment.NewLine, _masterPlaylist.ToString());
    }
}