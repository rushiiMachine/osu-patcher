using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

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

    #region Options

    #endregion
}