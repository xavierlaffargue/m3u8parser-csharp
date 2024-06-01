using M3U8Parser.Attributes.BaseAttribute;
using NUnit.Framework;

namespace M3U8Parser.Tests
{
	using M3U8Parser.ExtXType;
    using System;
	using System.IO;

	public class LoadMasterPlaylistTests
	{
		private MasterPlaylist _masterPlaylist = new ();

		[SetUp]
		public void Setup()
		{
			_masterPlaylist = MasterPlaylist.LoadFromFile(@"Sample" + Path.DirectorySeparatorChar + "manifest_1.m3u8");
		}

		[Test]
		public void ShouldBeHave5Medias()
		{
			Assert.AreEqual(5, _masterPlaylist.Medias.Count);
		}

		[Test]
		public void ShouldBeHave5Streams()
		{
			Assert.AreEqual(7, _masterPlaylist.Streams.Count);
		}

		[Test]
		public void ShouldBeHave5IFrameStreams()
		{
			Assert.AreEqual(6, _masterPlaylist.IFrameStreams.Count);
		}

		[Test]
		public void ShouldBeHlsVersion7()
		{
			Assert.AreEqual(7, _masterPlaylist.HlsVersion);
		}

		[Test]
		public void CheckFirstMedia()
		{
			Assert.AreEqual(MediaType.Audio, _masterPlaylist.Medias[0].Type);
			Assert.AreEqual("audio", _masterPlaylist.Medias[0].GroupId);
			Assert.AreEqual("English", _masterPlaylist.Medias[0].Name);
			Assert.AreEqual("eng", _masterPlaylist.Medias[0].Language);
			Assert.AreEqual(true, _masterPlaylist.Medias[0].AutoSelect);
			Assert.AreEqual(true, _masterPlaylist.Medias[0].Default);
			Assert.AreEqual("QualityLevels(192000)/Manifest(audio_eng_aacl,format=m3u8-aapl,filter=desktop)", _masterPlaylist.Medias[0].Uri);
		}

		[Test]
		public void AfterEditMediaTypeShouldBeChanged()
		{
			_masterPlaylist.Medias[0].Type = MediaType.Video;
			Assert.AreEqual(MediaType.Video, _masterPlaylist.Medias[0].Type);
		}

		[Test]
		public void AfterEditAutoSelectShouldBeChanged()
		{
			_masterPlaylist.Medias[0].AutoSelect = false;
			Assert.AreEqual(false, _masterPlaylist.Medias[0].AutoSelect);
		}

		[Test]
		public void AfterEditLanguageShouldBeChanged()
		{
			_masterPlaylist.Medias[0].Language = "fre";
			Assert.AreEqual("fre", _masterPlaylist.Medias[0].Language);
		}

		[Test]
		public void DefaultVersionShouldBe4()
		{
			var masterPlaylist = new MasterPlaylist();
			Assert.AreEqual(4, masterPlaylist.HlsVersion);
		}

		[Test]
		public void ParseClosedCaptionsMediaShouldBeOk()
		{
			var media = new Media("#EXT-X-MEDIA:TYPE=CLOSED-CAPTIONS,GROUP-ID=\"cc\",NAME=\"CEA608_CC\",AUTOSELECT=NO,DEFAULT=NO,INSTREAM-ID=\"CC1\"");
			Assert.AreEqual(MediaType.CloseCaptions, media.Type);
			Assert.AreEqual("cc", media.GroupId);
			Assert.AreEqual("CEA608_CC", media.Name);
			Assert.AreEqual(false, media.AutoSelect);
			Assert.AreEqual(false, media.Default);
			Assert.AreEqual("CC1", media.InstreamId);
			Assert.AreEqual(null, media.Language);
		}

		[Test]
		public void ParseVideoMediaShouldBeOk()
		{
			var media = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
			Assert.AreEqual(MediaType.Audio, media.Type);
			Assert.AreEqual("audio", media.GroupId);
			Assert.AreEqual("English", media.Name);
			Assert.AreEqual("eng", media.Language);
			Assert.AreEqual(true, media.AutoSelect);
			Assert.AreEqual(true, media.Default);
			Assert.AreEqual("uri/Manifest", media.Uri);
			Assert.AreEqual(null, media.InstreamId);
		}

        [Test]
        public void ParseStreamInfShouldBeOk()
        {
            var media = new StreamInf("#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=2778321,BANDWIDTH=3971374,VIDEO-RANGE=PQ,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-1\r\nsdr_720/prog_index.m3u8");
            Assert.AreEqual(2778321, media.AverageBandwidth);
            Assert.AreEqual(3971374, media.Bandwidth);
            Assert.AreEqual(VideoRangeType.PQ, media.VideoRange);
            Assert.AreEqual("hvc1.2.4.L123.B0", media.Codecs);
            Assert.AreEqual(23.976, media.FrameRate);
            Assert.AreEqual(HdcpLevelType.TYPE_1, media.HdcpLevel);
            Assert.AreEqual(1280, media.Resolution.Width);
            Assert.AreEqual(720, media.Resolution.Height);
            Assert.AreEqual("sdr_720/prog_index.m3u8", media.Uri);
        }
        
        [Test]
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

            Assert.AreEqual(1920, media.Resolution.Width);
            Assert.AreEqual(1080, media.Resolution.Height);
        }
        
		[Test]
		public void ParseAndToStringMediaShouldBeEqual()
		{
			var media = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
			Assert.AreEqual("#EXT-X-MEDIA:AUTOSELECT=YES,DEFAULT=YES,GROUP-ID=\"audio\",LANGUAGE=\"eng\",TYPE=AUDIO,NAME=\"English\",URI=\"uri/Manifest\"", media.ToString());
		}

		[Test]
		public void ParseAndToStringMediaShouldBeEqualWithDoubleQuote()
		{
			var media = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English \\\"Original\\\"\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
			Assert.AreEqual("#EXT-X-MEDIA:AUTOSELECT=YES,DEFAULT=YES,GROUP-ID=\"audio\",LANGUAGE=\"eng\",TYPE=AUDIO,NAME=\"English \\\"Original\\\"\",URI=\"uri/Manifest\"", media.ToString());
		}

		[Test]
		public void ParseAndToStringStreamShouldBeEqual()
		{
			var media = new StreamInf("#EXT-X-STREAM-INF:BANDWIDTH=3971374,AVERAGE-BANDWIDTH=2778321,VIDEO-RANGE=SDR,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,HDCP-LEVEL=NONE\r\nsdr_720/prog_index.m3u8");
			Assert.AreEqual("#EXT-X-STREAM-INF:BANDWIDTH=3971374,AVERAGE-BANDWIDTH=2778321,VIDEO-RANGE=SDR,CODECS=\"hvc1.2.4.L123.B0\",RESOLUTION=1280x720,FRAME-RATE=23.976,HDCP-LEVEL=NONE\r\nsdr_720/prog_index.m3u8", media.ToString());
		}

		[Test]
		public void OptionnalAttributesShouldBeNullable()
		{
			var media = new StreamInf("#EXT-X-STREAM-INF:BANDWIDTH=2778321,CODECS=\"hvc1.2.4.L123.B0\"\r\nsdr_720/prog_index.m3u8");
			Assert.AreEqual(media.AverageBandwidth, null);
			Assert.AreEqual(media.FrameRate, null);
			Assert.AreEqual(media.VideoRange, null);
			Assert.AreEqual(media.Resolution, null);
			Assert.AreEqual(media.HdcpLevel, null);
		}

		[Test]
		public void ParseAndToStringIFrameStreamShouldBeEqual()
		{
			var media = new IframeStreamInf("#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=621335,CODECS=\"avc1.42c00d\",RESOLUTION=416x234,URI=\"QualityLevels(399992)/Manifest(video,format=m3u8-aapl,filter=desktop,type=keyframes)\"");
			Assert.AreEqual("#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=621335,CODECS=\"avc1.42c00d\",RESOLUTION=416x234,URI=\"QualityLevels(399992)/Manifest(video,format=m3u8-aapl,filter=desktop,type=keyframes)\"", media.ToString());
		}

		[Test]
		public void MasterPlaylistShouldBeContainExtM3U()
		{
			Assert.That(_masterPlaylist.ToString(), Does.Contain("#EXTM3U" + Environment.NewLine));
		}

		[Test]
		public void MasterPlaylistShouldBeContainVersionExtM3U()
		{
			Assert.That(_masterPlaylist.ToString(), Does.Contain("#EXT-X-VERSION:7" + Environment.NewLine));
		}
    }
}