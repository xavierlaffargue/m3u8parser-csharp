using System;
using M3U8Parser;

namespace ConsoleApp1;

using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        //var masterPlaylist = new MasterPlaylist();
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
        Console.WriteLine(masterPlaylist.ToString());
        Console.ReadKey();
    }
}