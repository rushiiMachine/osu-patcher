using System;
using System.Diagnostics;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance.ROsu;
using Osu.Utils;

namespace Osu.Patcher.Hook;

[UsedImplicitly]
public static class Hook
{
    private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
    private static Harmony _harmony = null!;

    /// <summary>
    ///     Entry point into the hook called by the injector.
    /// </summary>
    [UsedImplicitly]
    public static int Initialize(string _)
    {
        ConsoleHook.Initialize();

        try
        {
            _harmony = new Harmony("osu!patcher");
            InitializePatches(_harmony);

            Notifications.ShowMessage(
                "osu!patcher initialized!",
                NotificationColor.Neutral,
                5000,
                () => { Process.Start(GithubUrl); }
            );

            var difficulty = new OsuDifficultyAttributes
            {
                Stars = 727.727,
                MaxCombo = 2500,
                SpeedNoteCount = 10,
                ApproachRate = 11,
                OverallDifficulty = 11,
                HealthRate = 10,
                AimSkill = 10,
                SpeedSkill = 100,
                FlashlightSkill = 0,
                SliderSkill = 20,
                CircleCount = 2000,
                SliderCount = 250,
                SpinnerCount = 250,
            };
            var score = new OsuScoreState
            {
                ScoreMaxCombo = 2499,
                Score300s = 2499,
                Score100s = 1,
                Score50s = 0,
                ScoreMisses = 0,
            };
            
            Console.WriteLine(new OsuPerformance(difficulty).CalculateScore(score).TotalPP);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ShowErrorNotification();
        }

        return 0;
    }

    /// <summary>
    ///     For every type annotated with <c>[HarmonyPatch]</c> in this assembly, initialize the patch.
    /// </summary>
    /// <param name="harmony">Harmony patcher instance</param>
    private static void InitializePatches(Harmony harmony)
    {
        foreach (var type in AccessTools.GetTypesFromAssembly(typeof(Hook).Assembly))
        {
            try
            {
                // This executes for every type and ignores if no [HarmonyPatch] is present
                harmony.CreateClassProcessor(type).Patch();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to initialize patch {type.Name}");
                Console.WriteLine(e);
                ShowErrorNotification();
            }
        }
    }

    /// <summary>
    ///     Show a generic error notification to the user.
    /// </summary>
    private static void ShowErrorNotification()
    {
        try
        {
            Notifications.ShowMessage(
                "osu!patcher experienced an error! Click to report.",
                NotificationColor.Error,
                20000,
                () => { Process.Start($"{GithubUrl}/issues"); });
        }
        catch (Exception e2)
        {
            Console.WriteLine("Failed to show error notification!");
            Console.WriteLine(e2);
        }
    }
}