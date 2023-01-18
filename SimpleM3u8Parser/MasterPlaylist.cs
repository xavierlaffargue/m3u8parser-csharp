using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleM3u8Parser;

public class MasterPlaylist
{
    private readonly int _hlsVersion;

    public MasterPlaylist(int hlsVersion = 4)
    {
        _hlsVersion = hlsVersion;
    }

    public List<Media> Medias { get; private set; } = new();
    public List<IframeStreamInf> IFrameStreams { get; private set; } = new();
    public List<StreamInf> Streams { get; private set; } = new();

    public static MasterPlaylist LoadFromFile(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"File not found : {path}");

        return LoadFromText(File.ReadAllText(path));
    }

    public void AddMedia(Media media)
    {
        Medias.Add(media);
    }

    public void AddMedias(IEnumerable<Media> medias)
    {
        Medias.AddRange(medias);
    }

    public void AddStream(StreamInf streamInf)
    {
        Streams.Add(streamInf);
    }

    public void AddStreams(IEnumerable<StreamInf> streamInfs)
    {
        Streams.AddRange(streamInfs);
    }

    public void AddIframeStreamInf(IframeStreamInf iframeStreamInf)
    {
        IFrameStreams.Add(iframeStreamInf);
    }

    public void AddIframeStreamInfs(IEnumerable<IframeStreamInf> iframeStreamInfs)
    {
        IFrameStreams.AddRange(iframeStreamInfs);
    }

    public static MasterPlaylist LoadFromText(string text)
    {
        List<Media> medias = new();
        List<IframeStreamInf> iFrameStreams = new();
        List<StreamInf> streams = new();

        var l = Regex.Split(text, "(?=#EXT-X)");

        foreach (var line in l)
        {
            if (line.StartsWith(Media.Prefix))
            {
                var media = new Media(line);
                medias.Add(media);
            }

            if (line.StartsWith(IframeStreamInf.Prefix))
            {
                var streaminf = new IframeStreamInf(line);
                iFrameStreams.Add(streaminf);
            }

            if (line.StartsWith(StreamInf.Prefix))
            {
                var stream = new StreamInf(line);
                streams.Add(stream);
            }
        }

        return new MasterPlaylist()
        {
            Medias = medias,
            Streams = streams,
            IFrameStreams = iFrameStreams
        };
    }

    public override string ToString()
    {
        var strBuilder = new StringBuilder();

        strBuilder.AppendLine("#EXTM3U");
        strBuilder.AppendLine($"#EXT-X-VERSION:{_hlsVersion}");

        strBuilder.AppendLine();

        if (Medias.Count > 0)
        {
            foreach (var media in Medias) strBuilder.AppendLine(media.ToString());
            strBuilder.AppendLine();
        }

        if (IFrameStreams.Count > 0)
        {
            foreach (var iframeStream in IFrameStreams) strBuilder.AppendLine(iframeStream.ToString());
            strBuilder.AppendLine();
        }

        if (Streams.Count > 0)
        {
            foreach (var stream in Streams) strBuilder.AppendLine(stream.ToString());
            strBuilder.AppendLine();
        }

        return strBuilder.ToString().TrimEnd();
    }
}