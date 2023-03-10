# M3U8 HLS Parser C#

## Installation
Package NuGet : https://www.nuget.org/packages/M3U8Parser/

```dotnet add package M3U8Parser```

## Documentation

M3U8Parser makes it easy to read, edit and create a m3u8 file. 
* M3U8Parser does not try to validate your file, it's your responsability to follow RFC.
* Require .netstandard2.0 / .net6

Usage:

Read a file and edit it
```csharp
    // Load a file
    var masterPlaylist = MasterPlaylist.LoadFromFile("master.m3u8");

    // Example, list all stream uri
    foreach(var stream in masterPlaylist.Streams)
    {
        Console.WriteLine("Uri:" + stream.Uri);
    }
    
    // Delete CLOSED-CAPTIONS type EXT-X-MEDIA
    masterPlaylist.Medias.RemoveAll(m => m.Type.Equals(MediaType.CloseCaptions));
```

Or your can create a master file
```csharp
    var masterPlaylist = new MasterPlaylist(hlsVersion: 4);

    // Add EXT-X-MEDIA
    masterPlaylist.AddMedia(new Media
    {
        Default = true,
        AutoSelect = true,
        Language = "eng",
        Name = "English",
        Type = MediaType.Audio,
        GroupId = "audio"
    });

    // Add another EXT-X-MEDIA but from text, yes it works!

    var mediaWithDv = new Media("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"audio\",NAME=\"English DV\",LANGUAGE=\"eng\",AUTOSELECT=YES,DEFAULT=NO");
    mediaWithDv.Characteristics = "public.accessibility.describes-video";

    masterPlaylist.AddMedia(mediaWithDv);

    // Add EXT-X-STREAM
    masterPlaylist.AddStream(new StreamInf()
    {
        Codecs = "avc1.4d401f,mp4a.40.2",
        Bandwidth = 900000,
        Uri = "v0.m3u8"
    });
    
    masterPlaylist.AddStream(new StreamInf()
    {
        Codecs = "avc1.4d401f,mp4a.40.2",
        Bandwidth = 1000000,
        Uri = "v1.m3u8"
    });
```

This code should produce the following master playlist:
```
#EXTM3U
#EXT-X-VERSION:4

#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID="audio",NAME="English",LANGUAGE="eng",AUTOSELECT=YES,DEFAULT=YES
#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID="audio",NAME="English DV",LANGUAGE="eng",AUTOSELECT=YES,DEFAULT=NO,CHARACTERISTICS="public.accessibility.describes-video"

#EXT-X-STREAM-INF:BANDWIDTH=900000,CODECS="avc1.4d401f,mp4a.40.2"
v0.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=1000000,CODECS="avc1.4d401f,mp4a.40.2"
v1.m3u8
```

Limitation: 
* Alpha version : Only master is supported (playlist be coming)


## Supported tags
The following tags should be fully supported:

### Basics
| TAGS  |
| ------------- |
| EXTM3U |
| EXT-X-VERSION |

### Master Playlist Tags
| TAGS  | ATTRIBUTE                                                                                                           |
| ------------- |---------------------------------------------------------------------------------------------------------------------|
| EXT-X-MEDIA  | GROUP-ID, AUTOSELECT, DEFAULT, LANGUAGE, NAME, TYPE, URI, CHARACTERISTICS, INSTREAM-ID                              |
| EXT-X-STREAM-INF  | BANDWIDTH, CODECS, RESOLUTION, URI,  SUBTITLES, VIDEO-RANGE, HDCP-LEVEL, AVERAGE-BANDWIDTH                                                                                  |
| EXT-X-I-FRAME-STREAM-INF  | BANDWIDTH, CODECS, RESOLUTION, AUDIO, VIDEO, CLOSED-CAPTIONS |

