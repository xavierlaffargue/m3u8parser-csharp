# M3U8 HLS Parser C#

A simple, fast, and lightweight M3U8 parser and generator for C#. It supports parsing, modifying, and generating both Multivariant (Master) and Media HLS playlists, including modern Live and Low-Latency HLS (LL-HLS) extensions.

## Installation

Install the package via NuGet:

```bash
dotnet add package M3U8Parser
```

## Features & Philosophy

* **Target Frameworks:** Compatible with `.NET Standard 2.0` and `.NET 10.0` (as well as `.NET 9.0`).
* **Non-Validating Parser:** M3U8Parser **does not try to validate your files** against the HLS RFCs. It is strictly a reader, editor, and generator. The parsed attributes and values are exposed directly into corresponding C# types (e.g., `string`, `decimal?`, `int?`). Ensuring compliance with RFC 8216 / RFC 8216 Bis is the user's responsibility.
* **Highly Stateful:** Correctly groups stateful segment-level tags (such as `#EXT-X-KEY`, `#EXT-X-MAP`, `#EXT-X-PROGRAM-DATE-TIME`, `#EXT-X-GAP`, and `#EXT-X-PART`) into parent segment objects.
* **Modern LL-HLS Support:** Full support for low-latency HLS tags such as content steering, preloading hints, skip, server control, and rendition reports.

---

## Usage Examples

### 1. Multivariant (Master) Playlists

#### Read and Edit an Existing Master Playlist

```csharp
using M3U8Parser;

// Load from a file or directly from text
var masterPlaylist = MasterPlaylist.LoadFromFile("master.m3u8");

// List all stream URIs and bandwidths
foreach (var stream in masterPlaylist.Streams)
{
    Console.WriteLine($"URI: {stream.Uri}, Bandwidth: {stream.Bandwidth}");
}

// Delete CLOSED-CAPTIONS type EXT-X-MEDIA
masterPlaylist.Medias.RemoveAll(m => m.Type == MediaType.CloseCaptions);

// Serialize back to M3U8 text representation
string output = masterPlaylist.ToString();
```

#### Create a Master Playlist from Scratch

```csharp
using M3U8Parser;
using M3U8Parser.Tags.MultivariantPlaylist;

var masterPlaylist = new MasterPlaylist(hlsVersion: 7);

// Add EXT-X-MEDIA
masterPlaylist.Medias.Add(new Media
{
    Default = true,
    AutoSelect = true,
    Language = "eng",
    Name = "English Audio",
    Type = MediaType.Audio,
    GroupId = "audio-group"
});

// Add EXT-X-STREAM-INF
masterPlaylist.Streams.Add(new StreamInf
{
    Codecs = "avc1.4d401f,mp4a.40.2",
    Bandwidth = 1000000,
    AverageBandwidth = 900000,
    Resolution = new ResolutionType { Width = 1280, Height = 720 },
    FrameRate = 29.97m,
    Audio = "audio-group",
    Uri = "720p/manifest.m3u8"
});

// You can also instantiate a tag directly from raw text!
var rawMedia = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio-group\",NAME=\"English DV\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=NO");
rawMedia.Characteristics = "public.accessibility.describes-video";
masterPlaylist.Medias.Add(rawMedia);

Console.WriteLine(masterPlaylist.ToString());
```

---

### 2. Media Playlists

#### Read and Analyze a Media Playlist

```csharp
using M3U8Parser;

var mediaPlaylist = MediaPlaylist.LoadFromFile("playlist.m3u8");

Console.WriteLine($"Target Duration: {mediaPlaylist.TargetDuration}");
Console.WriteLine($"Media Sequence: {mediaPlaylist.MediaSequence}");
Console.WriteLine($"Playlist Type: {mediaPlaylist.PlaylistType}");

// Access parsed media segments
foreach (var mediaSegment in mediaPlaylist.MediaSegments)
{
    if (mediaSegment.Key != null)
    {
        Console.WriteLine($"Encryption Key Method: {mediaSegment.Key.Method}");
    }

    foreach (var segment in mediaSegment.Segments)
    {
        Console.WriteLine($"Segment URI: {segment.Uri}, Duration: {segment.Duration}s");
        if (segment.Gap)
        {
            Console.WriteLine("Warning: This segment is flagged as a GAP!");
        }
    }
}
```

#### Generate a Media Playlist with Segments and LL-HLS Elements

```csharp
using M3U8Parser;
using M3U8Parser.Tags.Basic;
using M3U8Parser.Tags.MediaPlaylist;
using M3U8Parser.Tags.MediaSegment;

var mediaPlaylist = new MediaPlaylist
{
    HlsVersion = 13,
    TargetDuration = 6,
    MediaSequence = 101,
    HasEndList = true
};

// Add Define tags
mediaPlaylist.Defines.Add(new Define { Name = "VAR_NAME", Value = "VAL_VALUE" });

// Add Server Control (LL-HLS)
mediaPlaylist.ServerControl = new ServerControl
{
    CanSkipUntil = 36.0m,
    CanSkipDateRanges = true,
    HoldBack = 18.0m,
    PartHoldBack = 3.0m,
    CanBlockReload = true
};

// Create a MediaSegment with encryption keys & segments
var mediaSegment = new MediaSegment
{
    Key = new Key("#EXT-X-KEY:METHOD=AES-128,URI=\"https://key-server.com/key.bin\"")
};

// Add normal segments
var segment = new Segment
{
    Duration = 6.003,
    Uri = "segment101.ts",
    Title = "Segment 101 Title",
    Bitrate = 1500
};

// Add partial segment parts (LL-HLS)
segment.Parts.Add(new Part
{
    Duration = 1.0m,
    Independent = true,
    Uri = "part-101.1.mp4"
});

mediaSegment.Segments.Add(segment);
mediaPlaylist.MediaSegments.Add(mediaSegment);

Console.WriteLine(mediaPlaylist.ToString());
```

---

## Supported Tags & Attributes Reference

Here is a complete list of the HLS tags parsed and generated by the library.

### Basic Tags

| Tag | Associated C# Class / Property | Attributes / Notes |
| :--- | :--- | :--- |
| `#EXTM3U` | Root marker (Implicit) | Starting tag of any valid playlist. |
| `#EXT-X-VERSION` | `HlsVersion` | Format version of the playlist. |

### Multivariant Playlist Tags

| Tag | Associated C# Class / Property | Supported Attributes |
| :--- | :--- | :--- |
| `#EXT-X-MEDIA` | `Media` | `TYPE`, `GROUP-ID`, `LANGUAGE`, `NAME`, `DEFAULT`, `AUTOSELECT`, `CHANNELS`, `URI`, `CHARACTERISTICS`, `INSTREAM-ID`, `STABLE-RENDITION-ID`, `BIT-DEPTH`, `SAMPLE-RATE` |
| `#EXT-X-STREAM-INF` | `StreamInf` | `BANDWIDTH`, `AVERAGE-BANDWIDTH`, `CODECS`, `RESOLUTION`, `FRAME-RATE`, `CLOSED-CAPTIONS`, `AUDIO`, `VIDEO`, `SUBTITLES`, `VIDEO-RANGE`, `HDCP-LEVEL`, `SCORE`, `SUPPLEMENTAL-CODECS`, `ALLOWED-CPC`, `STABLE-VARIANT-ID`, `PATHWAY-ID`, `REQ-VIDEO-LAYOUT`, and the playlist `URI` (on the following line) |
| `#EXT-X-I-FRAME-STREAM-INF` | `IframeStreamInf` | `BANDWIDTH`, `AVERAGE-BANDWIDTH`, `CODECS`, `RESOLUTION`, `VIDEO`, `VIDEO-RANGE`, `HDCP-LEVEL`, `SCORE`, `SUPPLEMENTAL-CODECS`, `ALLOWED-CPC`, `STABLE-VARIANT-ID`, `PATHWAY-ID`, `URI` |
| `#EXT-X-CONTENT-STEERING` | `ContentSteering` | `SERVER-URI`, `PATHWAY-ID` |

### Media Playlist Tags

| Tag | Associated C# Class / Property | Supported Attributes / Notes |
| :--- | :--- | :--- |
| `#EXT-X-TARGETDURATION` | `TargetDuration` | Upper limit of duration of individual media segments (seconds). |
| `#EXT-X-MEDIA-SEQUENCE` | `MediaSequence` | The sequence number of the first segment in the playlist. |
| `#EXT-X-PLAYLIST-TYPE` | `PlaylistType` | `EVENT` or `VOD`. |
| `#EXT-X-I-FRAMES-ONLY` | `IFrameOnly` | Indicates that the playlist contains only I-frame segments. |
| `#EXT-X-INDEPENDENT-SEGMENTS` | `IndependentSegments` | Flag indicating independent segment signaling. |
| `#EXT-X-ENDLIST` | `HasEndList` | Signifies that no more media segments will be added to the playlist. |
| `#EXT-X-SERVER-CONTROL` | `ServerControl` | `CAN-SKIP-UNTIL`, `CAN-SKIP-DATERANGES`, `HOLD-BACK`, `PART-HOLD-BACK`, `CAN-BLOCK-RELOAD` |
| `#EXT-X-SKIP` | `Skip` | `SKIPPED-SEGMENTS`, `RECENTLY-REMOVED-DATERANGES` |
| `#EXT-X-PART-INF` | `PartInf` | `PART-TARGET` |
| `#EXT-X-PRELOAD-HINT` | `PreloadHints` | `TYPE`, `URI`, `BYTERANGE-START`, `BYTERANGE-LENGTH` |
| `#EXT-X-RENDITION-REPORT` | `RenditionReports` | `URI`, `LAST-MSN`, `LAST-PART` |
| `#EXT-X-DEFINE` | `Defines` | `NAME`, `VALUE`, `IMPORT`, `QUERYPARAM` |
| `#EXT-X-DATERANGE` | `DateRanges` | `ID`, `CLASS`, `START-DATE`, `END-DATE`, `DURATION`, `PLANNED-DURATION`, `SCTE35-CMD`, `SCTE35-OUT`, `SCTE35-IN`, `END-ON-NEXT`, plus standard HLS Interstitial properties (`X-ASSET-URI`, `X-ASSET-LIST`, `X-RESUME-OFFSET`, `X-PLAYOUT-LIMIT`, `X-SNAP`, `X-RESTRICT`, `X-CONTENT-MAY-VARY`, `X-TIMELINE-OCCUPIES`, `X-TIMELINE-STYLE`, `X-SKIP-CONTROL-OFFSET`, `X-SKIP-CONTROL-DURATION`, `X-SKIP-CONTROL-LABEL-ID`, `X-URI`, `X-TARGET-ID`, `X-TARGET-CLASS`). |

### Media Segment Tags (Contained within Segments)

| Tag | Associated C# Class / Property | Supported Attributes / Notes |
| :--- | :--- | :--- |
| `#EXTINF` | `Segment.Duration` / `Segment.Title` | Segment duration in decimal seconds, optional title, followed by the actual segment `URI` on the next line. |
| `#EXT-X-BYTERANGE` | `Segment.ByteRangeLength` / `Segment.ByteRangeStartSubRange` | Identifies a sub-range of a resource (length@start). |
| `#EXT-X-KEY` | `Key` (associated with `MediaSegment`) | `METHOD` (NONE, AES-128, SAMPLE-AES), `URI`, `IV`, `KEYFORMAT`, `KEYFORMATVERSIONS` |
| `#EXT-X-MAP` | `Map` / `Segment.Map` | `URI`, `BYTERANGE` |
| `#EXT-X-PROGRAM-DATE-TIME` | `Segment.ProgramDateTime` | Absolute date and/or time of the segment's start. |
| `#EXT-X-GAP` | `Segment.Gap` | Identifies missing segments. |
| `#EXT-X-BITRATE` | `Segment.Bitrate` | Declares the exact or average bitrate of the segment. |
| `#EXT-X-PART` | `Part` (associated with `Segment`) | `DURATION`, `URI`, `INDEPENDENT`, `BYTERANGE`, `GAP` |

---

## What is NOT Supported / Out of Scope

1. **Validation Checks:** The library does not check if properties like `TARGETDURATION` are violated by segments having longer durations. It does not validate structural constraints, check if references are valid, or verify format constraints of cryptographic keys.
2. **Built-in Downloading or Networking:** Downloading referenced files or segments is out of scope. This is strictly a manifest parser and serializer.
3. **Custom / Private Tags:** Custom tags that do not inherit or align with the internal `AbstractTag` implementation will not be parsed automatically. However, standard unknown tags can often be extracted or preserved depending on the parser context, or represented as unquoted strings.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
