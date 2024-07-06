namespace M3U8Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.ExtXType;

    public class MasterPlaylist
    {
        private const int DefaultHlsVersion = 4;

        public MasterPlaylist(int hlsVersion = DefaultHlsVersion)
        {
            HlsVersion = hlsVersion;
        }

        public int HlsVersion { get; set; }

        public List<Media> Medias { get; set; } = new ();

        public List<IframeStreamInf> IFrameStreams { get; set; } = new ();

        public List<StreamInf> Streams { get; set; } = new ();

        public static MasterPlaylist LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found : {path}");
            }

            return LoadFromText(File.ReadAllText(path));
        }

        public static MasterPlaylist LoadFromText(string text)
        {
            List<Media> medias = new ();
            List<IframeStreamInf> iFrameStreams = new ();
            List<StreamInf> streams = new ();
            var hlsVersion = DefaultHlsVersion;

            var matchHlsVersion = Regex.Match(text, "(?<=#EXT-X-VERSION:)(.*?)(?<=$)", RegexOptions.Multiline);
            if (matchHlsVersion.Success)
            {
                var info = matchHlsVersion.Groups[0].Value;
                hlsVersion = int.Parse(info);
            }

            var l = Regex.Split(text, "(?=#EXT)");

            foreach (var line in l)
            {
                if (line.StartsWith(Media.Prefix))
                {
                    medias.Add(new Media(line));
                }
                else if (line.StartsWith(IframeStreamInf.Prefix))
                {
                    iFrameStreams.Add(new IframeStreamInf(line));
                }
                else if (line.StartsWith(StreamInf.Prefix))
                {
                    streams.Add(new StreamInf(line));
                }
            }

            return new MasterPlaylist(hlsVersion)
            {
                Medias = medias,
                Streams = streams,
                IFrameStreams = iFrameStreams,
            };
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("#EXTM3U");
            strBuilder.AppendLine($"#EXT-X-VERSION:{HlsVersion}");

            strBuilder.AppendLine();

            if (Medias.Count > 0)
            {
                foreach (var media in Medias)
                {
                    strBuilder.AppendLine(media.ToString());
                }

                strBuilder.AppendLine();
            }

            if (IFrameStreams.Count > 0)
            {
                foreach (var iframeStream in IFrameStreams)
                {
                    strBuilder.AppendLine(iframeStream.ToString());
                }

                strBuilder.AppendLine();
            }

            if (Streams.Count > 0)
            {
                foreach (var stream in Streams)
                {
                    strBuilder.AppendLine(stream.ToString());
                }

                strBuilder.AppendLine();
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}