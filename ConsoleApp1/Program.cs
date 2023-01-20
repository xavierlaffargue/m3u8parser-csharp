using System;
using M3U8Parser;

namespace ConsoleApp1;

using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        //var masterPlaylist = new MasterPlaylist();

        var masterPlaylist = MasterPlaylist.LoadFromFile(@"C:\Users\LAFFARGX\Desktop\gem_manifest.m3u8");

        masterPlaylist.Medias.RemoveAll(m => m.Type.Equals(MediaType.CloseCaptions) && m.Name is "CEA708_CC" or "CEA608_CC");
        Console.WriteLine(masterPlaylist.ToString());

        
        
        Console.ReadKey();
    }
}