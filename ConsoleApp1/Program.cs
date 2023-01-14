using System;

namespace ConsoleApp1
{
	using SimpleM3u8Parser;

	class Program
	{
		static void Main(string[] args)
		{
			var m = new Media()
				.SetLanguage("eng")
				.SetName("Anglais");

			var text = m.ToString();
			Console.WriteLine(text);

			var media = new Media(text).SetName("Anglais DV");
			
			Console.WriteLine(media.GetLanguage());
			Console.WriteLine(media.GetName());
			Console.ReadKey();
		}
	}
}