using NUnit.Framework;

namespace M3U8Parser.Tests;

public class LoadMasterPlaylistTests
{

	private MasterPlaylist _masterPlaylist;
	
	[SetUp]
	public void Setup()
	{
		_masterPlaylist = MasterPlaylist.LoadFromFile(@"Sample\gem_manifest.m3u8");
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
	public void ParseAndToStringShouldBeEqual()
	{
		var media = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"");
		Assert.AreEqual("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=YES,URI=\"uri/Manifest\"", media.ToString());
	}
}