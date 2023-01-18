using System;
using SimpleM3u8Parser;

namespace ConsoleApp1;

internal class Program
{
    private static void Main(string[] args)
    {
        var m1 = new MasterPlaylist();

        m1.AddMedia(new Media
        {
            Default = true,
            AutoSelect = true,
            Language = "fra",
            Name = "Français",
            Type = MediaType.Audio,
            GroupId = "audio-high"
        });

        m1.AddMedia(new Media
        {
            Default = false,
            AutoSelect = true,
            Language = "fra",
            Name = "Français",
            Type = MediaType.Audio,
            GroupId = "audio-high",
            Characteristics = "public.accessibility.describes-video"
        });

        m1.AddMedia(new Media
        {
            Default = true,
            AutoSelect = true,
            Language = "fra",
            Name = "Français",
            Type = MediaType.Audio,
            GroupId = "audio-low"
        });

        m1.AddMedia(new Media
        {
            Default = false,
            AutoSelect = true,
            Language = "fra",
            Name = "Français",
            Type = MediaType.Audio,
            GroupId = "audio-low",
            Characteristics = "public.accessibility.describes-video"
        });

        m1.AddStream(new StreamInf
        {
            Audio = "audio-high",
            Bandwidth = 150000,
            Codecs = "m4a.40.5",
            Uri = "playlist_audio.m3u8"
        });

        m1.AddStream(new StreamInf
        {
            Audio = "audio-low",
            Bandwidth = 70000,
            Codecs = "m4a.40.5",
            Uri = "playlist_audio.m3u8"
        });


        Console.WriteLine(m1.ToString());

        Console.ReadKey();
    }
}