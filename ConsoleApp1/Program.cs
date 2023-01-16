using System;

namespace ConsoleApp1
{
	using SimpleM3u8Parser;
	using System.IO;
	using System.Text.RegularExpressions;

	class Program
	{
		static void Main(string[] args)
		{
			var txt = File.ReadAllText(@"C:\Users\xav-s\OneDrive\Bureau\Sample.m3u8");
		


            var m1 = new MasterPlaylist(txt);

			foreach (var m in m1.Medias)
			{


				var text = m.ToString();
				Console.WriteLine(text);
				/*
				var media = new Media(text).SetName("Anglais DV").SetType(MediaType.AUDIO).SetAutoSelect(false);

				Console.WriteLine(media.GetLanguage());
				Console.WriteLine(media.GetName());
				Console.WriteLine(media.GetMediaType());
				Console.WriteLine(media.GetAutoSelect());*/

			}
            Console.ReadKey();
		}
	}
}