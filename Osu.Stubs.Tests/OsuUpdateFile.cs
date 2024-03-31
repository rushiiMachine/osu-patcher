using Newtonsoft.Json;

namespace Osu.Stubs.Tests;

public class OsuUpdateFile
{
    [JsonRequired]
    [JsonProperty("file_version")]
    public int FileVersion { get; set; }

    [JsonRequired]
    [JsonProperty("filesize")]
    public int FileSize { get; set; }

    [JsonRequired]
    [JsonProperty("filename")]
    public string FileName { get; set; } = null!;

    [JsonRequired]
    [JsonProperty("file_hash")]
    public string FileHash { get; set; } = null!;

    [JsonRequired]
    [JsonProperty("timestamp")]
    public string Timestamp { get; set; } = null!;

    [JsonRequired]
    [JsonProperty("url_full")]
    public string DownloadUrl { get; set; } = null!;
}