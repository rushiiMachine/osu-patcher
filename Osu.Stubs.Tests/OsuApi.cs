using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Osu.Stubs.Tests;

[PublicAPI]
public static class OsuApi
{
    [PublicAPI]
    public enum ReleaseStream
    {
        CuttingEdge,
        Stable40,
        Beta40,
    }

    private static readonly HttpClient Http = new();

    /// <summary>
    ///     Gets the latest release files for a specific release stream.
    /// </summary>
    public static async Task<List<OsuUpdateFile>> GetReleaseFiles(ReleaseStream stream)
    {
        Console.WriteLine("Fetching latest osu! update info");

        var url = $"https://osu.ppy.sh/web/check-updates.php" +
                  $"?action=check" +
                  $"&stream={stream.ToString().ToLower()}" +
                  $"&time={DateTime.Now.Ticks}";

        using var response = await Http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var bodyText = await response.Content.ReadAsStringAsync();
        if (bodyText == null) throw new Exception("Response returned no body");

        var releaseFiles = JsonConvert.DeserializeObject<List<OsuUpdateFile>>(bodyText);
        if (releaseFiles == null) throw new Exception("Failed to deserialize update files");

        return releaseFiles;
    }

    /// <summary>
    ///     Downloads the full osu! update file list to a specific directory.
    /// </summary>
    /// <param name="dir">An empty directory.</param>
    /// <param name="stream">The release stream to download.</param>
    public static async Task DownloadOsu(string dir, ReleaseStream stream = ReleaseStream.Stable40)
    {
        var updateFiles = await GetReleaseFiles(ReleaseStream.Stable40);

        foreach (var updateFile in updateFiles)
        {
            Console.WriteLine($"Downloading {updateFile.FileName}");

            await DownloadFile(
                updateFile.DownloadUrl,
                Path.Combine(dir, updateFile.FileName)
            );
        }
    }

    private static async Task DownloadFile(string url, string path)
    {
        using var response = await Http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var bodyStream = await response.Content.ReadAsStreamAsync();
        if (bodyStream == null) throw new Exception("Response returned no body");

        using var file = File.Create(path);
        await bodyStream.CopyToAsync(file);
    }
}