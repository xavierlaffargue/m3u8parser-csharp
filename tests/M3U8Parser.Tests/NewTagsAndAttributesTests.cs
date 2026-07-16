using System;
using System.Collections.Generic;
using M3U8Parser.Attributes.ValueType;
using M3U8Parser.Tags.Basic;
using M3U8Parser.Tags.MediaPlaylist;
using M3U8Parser.Tags.MediaSegment;
using M3U8Parser.Tags.MultivariantPlaylist;
using Xunit;

namespace M3U8Parser.Tests
{
    public class NewTagsAndAttributesTests
    {
        [Fact]
        public void TestMasterPlaylistWithNewTagsAndAttributes()
        {
            var manifestText = @"#EXTM3U
#EXT-X-VERSION:13
#EXT-X-CONTENT-STEERING:SERVER-URI=""https://example.com/steering"",PATHWAY-ID=""CDN-A""
#EXT-X-DEFINE:NAME=""VAR1"",VALUE=""VAL1""
#EXT-X-DEFINE:IMPORT=""VAR2""

#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=""audio"",NAME=""English"",DEFAULT=YES,AUTOSELECT=YES,STABLE-RENDITION-ID=""Audio-37262"",BIT-DEPTH=24,SAMPLE-RATE=48000

#EXT-X-STREAM-INF:BANDWIDTH=1280000,SCORE=9.5,SUPPLEMENTAL-CODECS=""dvh1.08.07/db4h"",ALLOWED-CPC=""com.example:HW"",STABLE-VARIANT-ID=""Video-128"",PATHWAY-ID=""CDN-A"",REQ-VIDEO-LAYOUT=""CH-STEREO,CH-MONO""
128/video.m3u8";

            var masterPlaylist = MasterPlaylist.LoadFromText(manifestText);

            // Assertions for version
            Assert.Equal(13, masterPlaylist.HlsVersion);

            // Assertions for Content Steering
            Assert.NotNull(masterPlaylist.ContentSteering);
            Assert.Equal("https://example.com/steering", masterPlaylist.ContentSteering.ServerUri);
            Assert.Equal("CDN-A", masterPlaylist.ContentSteering.PathwayId);

            // Assertions for Defines
            Assert.Equal(2, masterPlaylist.Defines.Count);
            Assert.Equal("VAR1", masterPlaylist.Defines[0].Name);
            Assert.Equal("VAL1", masterPlaylist.Defines[0].Value);
            Assert.Equal("VAR2", masterPlaylist.Defines[1].Import);

            // Assertions for Media new attributes
            Assert.Single(masterPlaylist.Medias);
            var media = masterPlaylist.Medias[0];
            Assert.Equal("Audio-37262", media.StableRenditionId);
            Assert.Equal(24, media.BitDepth);
            Assert.Equal(48000, media.SampleRate);

            // Assertions for StreamInf new attributes
            Assert.Single(masterPlaylist.Streams);
            var stream = masterPlaylist.Streams[0];
            Assert.Equal(9.5m, stream.Score);
            Assert.Equal("dvh1.08.07/db4h", stream.SupplementalCodecs);
            Assert.Equal("com.example:HW", stream.AllowedCpc);
            Assert.Equal("Video-128", stream.StableVariantId);
            Assert.Equal("CDN-A", stream.PathwayId);
            Assert.Equal("CH-STEREO,CH-MONO", stream.ReqVideoLayout);

            // Round-trip test
            var serialized = masterPlaylist.ToString();
            var roundTrip = MasterPlaylist.LoadFromText(serialized);

            Assert.Equal(masterPlaylist.HlsVersion, roundTrip.HlsVersion);
            Assert.Equal(masterPlaylist.ContentSteering.ServerUri, roundTrip.ContentSteering.ServerUri);
            Assert.Equal(masterPlaylist.ContentSteering.PathwayId, roundTrip.ContentSteering.PathwayId);
            Assert.Equal(masterPlaylist.Defines.Count, roundTrip.Defines.Count);
            Assert.Equal(masterPlaylist.Medias[0].StableRenditionId, roundTrip.Medias[0].StableRenditionId);
            Assert.Equal(masterPlaylist.Streams[0].Score, roundTrip.Streams[0].Score);
            Assert.Equal(masterPlaylist.Streams[0].StableVariantId, roundTrip.Streams[0].StableVariantId);
        }

        [Fact]
        public void TestMediaPlaylistWithNewTagsAndAttributes()
        {
            var manifestText = @"#EXTM3U
#EXT-X-VERSION:13
#EXT-X-DEFINE:NAME=""VAR_MEDIA"",VALUE=""VAL_MEDIA""
#EXT-X-SERVER-CONTROL:CAN-SKIP-UNTIL=36.0,CAN-SKIP-DATERANGES=YES,HOLD-BACK=18.0,PART-HOLD-BACK=3.0,CAN-BLOCK-RELOAD=YES
#EXT-X-PART-INF:PART-TARGET=1.0
#EXT-X-TARGETDURATION:6
#EXT-X-MEDIA-SEQUENCE:10
#EXT-X-SKIP:SKIPPED-SEGMENTS=3,RECENTLY-REMOVED-DATERANGES=""range-1	range-2""

#EXT-X-GAP
#EXT-X-BITRATE:1200
#EXT-X-PART:DURATION=1.0,INDEPENDENT=YES,URI=""part1.mp4""
#EXT-X-PART:DURATION=1.0,GAP=YES,URI=""part2.mp4""
#EXTINF:2.0,
segment13.ts

#EXT-X-PRELOAD-HINT:TYPE=PART,URI=""part3.mp4"",BYTERANGE-START=100,BYTERANGE-LENGTH=200
#EXT-X-RENDITION-REPORT:URI=""/1M/LL-HLS.m3u8"",LAST-MSN=274,LAST-PART=1";

            var mediaPlaylist = MediaPlaylist.LoadFromText(manifestText);

            // Assertions for HlsVersion & Defines
            Assert.Equal(13, mediaPlaylist.HlsVersion);
            Assert.Single(mediaPlaylist.Defines);
            Assert.Equal("VAR_MEDIA", mediaPlaylist.Defines[0].Name);
            Assert.Equal("VAL_MEDIA", mediaPlaylist.Defines[0].Value);

            // Assertions for ServerControl
            Assert.NotNull(mediaPlaylist.ServerControl);
            Assert.Equal(36.0m, mediaPlaylist.ServerControl.CanSkipUntil);
            Assert.True(mediaPlaylist.ServerControl.CanSkipDateRanges);
            Assert.Equal(18.0m, mediaPlaylist.ServerControl.HoldBack);
            Assert.Equal(3.0m, mediaPlaylist.ServerControl.PartHoldBack);
            Assert.True(mediaPlaylist.ServerControl.CanBlockReload);

            // Assertions for PartInf
            Assert.NotNull(mediaPlaylist.PartInf);
            Assert.Equal(1.0m, mediaPlaylist.PartInf.PartTarget);

            // Assertions for Skip
            Assert.NotNull(mediaPlaylist.Skip);
            Assert.Equal(3, mediaPlaylist.Skip.SkippedSegments);
            Assert.Equal("range-1\trange-2", mediaPlaylist.Skip.RecentlyRemovedDateRanges);

            // Assertions for Segment with new tags (Gap, Bitrate, Parts)
            Assert.Single(mediaPlaylist.MediaSegments);
            Assert.Single(mediaPlaylist.MediaSegments[0].Segments);
            var segment = mediaPlaylist.MediaSegments[0].Segments[0];
            Assert.True(segment.Gap);
            Assert.Equal(1200, segment.Bitrate);
            Assert.Equal(2, segment.Parts.Count);

            var part1 = segment.Parts[0];
            Assert.Equal(1.0m, part1.Duration);
            Assert.True(part1.Independent);
            Assert.Equal("part1.mp4", part1.Uri);

            var part2 = segment.Parts[1];
            Assert.Equal(1.0m, part2.Duration);
            Assert.True(part2.Gap);
            Assert.Equal("part2.mp4", part2.Uri);

            // Assertions for PreloadHints & RenditionReports
            Assert.Single(mediaPlaylist.PreloadHints);
            var hint = mediaPlaylist.PreloadHints[0];
            Assert.Equal("PART", hint.Type);
            Assert.Equal("part3.mp4", hint.Uri);
            Assert.Equal(100, hint.ByteRangeStart);
            Assert.Equal(200, hint.ByteRangeLength);

            Assert.Single(mediaPlaylist.RenditionReports);
            var report = mediaPlaylist.RenditionReports[0];
            Assert.Equal("/1M/LL-HLS.m3u8", report.Uri);
            Assert.Equal(274, report.LastMsn);
            Assert.Equal(1, report.LastPart);

            // Round-trip test
            var serialized = mediaPlaylist.ToString();
            var roundTrip = MediaPlaylist.LoadFromText(serialized);

            Assert.Equal(mediaPlaylist.HlsVersion, roundTrip.HlsVersion);
            Assert.Equal(mediaPlaylist.Defines[0].Name, roundTrip.Defines[0].Name);
            Assert.Equal(mediaPlaylist.ServerControl.CanSkipUntil, roundTrip.ServerControl.CanSkipUntil);
            Assert.Equal(mediaPlaylist.PartInf.PartTarget, roundTrip.PartInf.PartTarget);
            Assert.Equal(mediaPlaylist.Skip.SkippedSegments, roundTrip.Skip.SkippedSegments);
            Assert.Equal(mediaPlaylist.MediaSegments[0].Segments[0].Gap, roundTrip.MediaSegments[0].Segments[0].Gap);
            Assert.Equal(mediaPlaylist.MediaSegments[0].Segments[0].Bitrate, roundTrip.MediaSegments[0].Segments[0].Bitrate);
            Assert.Equal(mediaPlaylist.MediaSegments[0].Segments[0].Parts.Count, roundTrip.MediaSegments[0].Segments[0].Parts.Count);
            Assert.Equal(mediaPlaylist.PreloadHints[0].Type, roundTrip.PreloadHints[0].Type);
            Assert.Equal(mediaPlaylist.RenditionReports[0].Uri, roundTrip.RenditionReports[0].Uri);
        }

        [Fact]
        public void TestDateRangeAndHlsInterstitials()
        {
            var manifestText = @"#EXTM3U
#EXT-X-VERSION:13
#EXT-X-TARGETDURATION:6
#EXT-X-DATERANGE:ID=""ad1"",CLASS=""com.apple.hls.interstitial"",START-DATE=""2020-01-02T21:55:44.000Z"",CUE=""PRE,POST"",DURATION=15.0,PLANNED-DURATION=15.0,END-ON-NEXT=YES,X-ASSET-URI=""http://example.com/ad1.m3u8"",X-ASSET-LIST=""http://example.com/adv.json"",X-RESUME-OFFSET=0,X-PLAYOUT-LIMIT=15.0,X-SNAP=""OUT,IN"",X-RESTRICT=""SKIP,JUMP"",X-CONTENT-MAY-VARY=""YES"",X-TIMELINE-OCCUPIES=""POINT"",X-TIMELINE-STYLE=""HIGHLIGHT"",X-SKIP-CONTROL-OFFSET=2,X-SKIP-CONTROL-DURATION=5,X-SKIP-CONTROL-LABEL-ID=""Exit-Label"",X-URI=""http://preload.com"",X-TARGET-ID=""target-1"",X-TARGET-CLASS=""class-1""

#EXTINF:6,
main1.ts";

            var mediaPlaylist = MediaPlaylist.LoadFromText(manifestText);

            // Assertions for DateRanges
            Assert.Single(mediaPlaylist.DateRanges);
            var dateRange = mediaPlaylist.DateRanges[0];

            Assert.Equal("ad1", dateRange.Id);
            Assert.Equal("com.apple.hls.interstitial", dateRange.Class);
            Assert.Equal("2020-01-02T21:55:44.000Z", dateRange.StartDate);
            Assert.Equal("PRE,POST", dateRange.Cue);
            Assert.Equal(15.0m, dateRange.Duration);
            Assert.Equal(15.0m, dateRange.PlannedDuration);
            Assert.Equal("YES", dateRange.EndOnNext);

            // Assertions for Interstitials
            Assert.Equal("http://example.com/ad1.m3u8", dateRange.XAssetUri);
            Assert.Equal("http://example.com/adv.json", dateRange.XAssetList);
            Assert.Equal(0m, dateRange.XResumeOffset);
            Assert.Equal(15.0m, dateRange.XPlayoutLimit);
            Assert.Equal("OUT,IN", dateRange.XSnap);
            Assert.Equal("SKIP,JUMP", dateRange.XRestrict);
            Assert.Equal("YES", dateRange.XContentMayVary);
            Assert.Equal("POINT", dateRange.XTimelineOccupies);
            Assert.Equal("HIGHLIGHT", dateRange.XTimelineStyle);

            // Assertions for Skip control
            Assert.Equal(2, dateRange.XSkipControlOffset);
            Assert.Equal(5, dateRange.XSkipControlDuration);
            Assert.Equal("Exit-Label", dateRange.XSkipControlLabelId);

            // Assertions for Preloading
            Assert.Equal("http://preload.com", dateRange.XUri);
            Assert.Equal("target-1", dateRange.XTargetId);
            Assert.Equal("class-1", dateRange.XTargetClass);

            // Round-trip test
            var serialized = mediaPlaylist.ToString();
            var roundTrip = MediaPlaylist.LoadFromText(serialized);

            Assert.Single(roundTrip.DateRanges);
            var roundTripDateRange = roundTrip.DateRanges[0];

            Assert.Equal(dateRange.Id, roundTripDateRange.Id);
            Assert.Equal(dateRange.Class, roundTripDateRange.Class);
            Assert.Equal(dateRange.StartDate, roundTripDateRange.StartDate);
            Assert.Equal(dateRange.Cue, roundTripDateRange.Cue);
            Assert.Equal(dateRange.Duration, roundTripDateRange.Duration);
            Assert.Equal(dateRange.PlannedDuration, roundTripDateRange.PlannedDuration);
            Assert.Equal(dateRange.EndOnNext, roundTripDateRange.EndOnNext);
            Assert.Equal(dateRange.XAssetUri, roundTripDateRange.XAssetUri);
            Assert.Equal(dateRange.XAssetList, roundTripDateRange.XAssetList);
            Assert.Equal(dateRange.XResumeOffset, roundTripDateRange.XResumeOffset);
            Assert.Equal(dateRange.XPlayoutLimit, roundTripDateRange.XPlayoutLimit);
            Assert.Equal(dateRange.XSnap, roundTripDateRange.XSnap);
            Assert.Equal(dateRange.XRestrict, roundTripDateRange.XRestrict);
            Assert.Equal(dateRange.XContentMayVary, roundTripDateRange.XContentMayVary);
            Assert.Equal(dateRange.XTimelineOccupies, roundTripDateRange.XTimelineOccupies);
            Assert.Equal(dateRange.XTimelineStyle, roundTripDateRange.XTimelineStyle);
            Assert.Equal(dateRange.XSkipControlOffset, roundTripDateRange.XSkipControlOffset);
            Assert.Equal(dateRange.XSkipControlDuration, roundTripDateRange.XSkipControlDuration);
            Assert.Equal(dateRange.XSkipControlLabelId, roundTripDateRange.XSkipControlLabelId);
            Assert.Equal(dateRange.XUri, roundTripDateRange.XUri);
            Assert.Equal(dateRange.XTargetId, roundTripDateRange.XTargetId);
            Assert.Equal(dateRange.XTargetClass, roundTripDateRange.XTargetClass);
        }
    }
}
