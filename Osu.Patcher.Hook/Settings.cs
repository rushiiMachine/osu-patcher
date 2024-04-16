using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Osu.Patcher.Hook.Patches.LivePerformance;

namespace Osu.Patcher.Hook;

/// <summary>
///     (De)serializable settings model to handle loading and saving settings to disk.
/// </summary>
[Serializable]
public class Settings
{
    private const string OptionsFilename = "osu!patcher.xml";
    private static readonly XmlSerializer Serializer = new(typeof(Settings));

    /// <summary>
    ///     An instance of settings representing the default values.
    ///     This should not be modified at runtime.
    /// </summary>
    [XmlIgnore]
    public static Settings Default = new();

    /// <summary>
    ///     Handles reading the patcher options from disk.
    /// </summary>
    public static Settings ReadFromDisk(string osuDir)
    {
        Debug.WriteLine("Reading patcher config from disk");

        var file = Path.Combine(osuDir, OptionsFilename);

        if (!File.Exists(file))
            return Default;

        using var fs = File.OpenRead(file);

        try
        {
            return (Settings)Serializer.Deserialize(fs);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Failed to parse config: {e}");
            return Default;
        }
    }

    /// <summary>
    ///     Handles writing the patcher options to disk.
    /// </summary>
    public static void WriteToDisk(Settings settings, string osuDir)
    {
        Debug.WriteLine("Writing patcher config to disk");

        Directory.CreateDirectory(osuDir);

        var file = Path.Combine(osuDir, OptionsFilename);
        using var fs = File.OpenWrite(file);

        try
        {
            Serializer.Serialize(fs, settings);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to write config to disk: {e}");
        }
    }

    #region Options

    public bool EnableModAudioPreview { get; set; } = true;
    public bool ShowPerformanceInGame { get; set; } = true;
    public bool ShowPerformanceOnLeaderboard { get; set; } = true;
    public PerformanceCalculatorType PerformanceCalculator { get; set; } = PerformanceCalculatorType.AkatsukiLimited;
    
    #endregion
}