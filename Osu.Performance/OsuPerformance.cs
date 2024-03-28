using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Osu.Performance;

[UsedImplicitly]
public class OsuPerformance : IDisposable
{
    private readonly ConcurrentQueue<PendingCalculation> _queue;
    private readonly CancellationTokenSource _queueTaskCancellation;
    private IntPtr _state = IntPtr.Zero;

    public OsuPerformance(string mapPath, uint mods)
    {
        _queue = new ConcurrentQueue<PendingCalculation>();
        _queueTaskCancellation = new CancellationTokenSource();

        Debug.WriteLine("Starting background task", nameof(OsuPerformance));
        Task.Factory.StartNew(
            () =>
            {
                _state = Native.InitializeOsuGradualPerformance(mapPath, mods);
                ProcessQueue();
            },
            _queueTaskCancellation.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default
        );
    }

    public void Dispose()
    {
        Debug.WriteLine("Disposing", "OsuPerformance");
        _queueTaskCancellation.Cancel();
        Native.DisposeGradualOsuPerformance(_state);
        OnNewCalculation = null;
        _state = IntPtr.Zero;
    }

    [UsedImplicitly]
    public event Action<double>? OnNewCalculation;

    /// <summary>
    ///     Queues a judgement to be processed later.
    /// </summary>
    /// <param name="judgement">
    ///     The final hit result of an object. If this is a slider tick, spinner tick, etc., then this
    ///     should be the <c>None</c> enum variant.
    /// </param>
    /// <param name="maxCombo">The currently highest max combo in the score.</param>
    [UsedImplicitly]
    public void AddJudgement(OsuJudgement judgement, uint maxCombo)
    {
        if (_queueTaskCancellation.IsCancellationRequested) return;

        _queue.Enqueue(new PendingCalculation
        {
            Judgement = judgement,
            MaxCombo = maxCombo,
        });
    }

    private async void ProcessQueue()
    {
        while (true)
        {
            if (_queueTaskCancellation.IsCancellationRequested) return;
            if (_state == IntPtr.Zero) // Not yet initialized
            {
                await Task.Delay(300);
                continue;
            }

            while (_queue.TryDequeue(out var item))
            {
                var performance = Native.CalculateGradualOsuPerformance(_state, item.Judgement, item.MaxCombo);

                if (performance < 0f)
                {
                    Debug.Fail("Cannot calculate performance after the end of a beatmap!");
                    break;
                }

                OnNewCalculation?.Invoke(performance);

                if (_queueTaskCancellation.IsCancellationRequested) return;
            }

            await Task.Delay(100);
        }
    }

    // /// <summary>
    // ///     Calculates the performance metrics of a score while and returns the complete info.
    // ///     If this is a failed score, or is in progress for whatever reason, then the end of the score will be
    // ///     calculated based on the sum of the amount of hits recorded in <paramref name="score" />.
    // /// </summary>
    // /// <param name="difficulty">The precalculated/cached difficulty attributes of a map.</param>
    // /// <param name="score">A completed (or failed) score's info on the associated map.</param>
    // /// <param name="mods">The set of mods that were used on this score.</param>
    // [UsedImplicitly]
    // public static OsuPerformanceInfo CalculateScore(
    //     OsuDifficultyAttributes difficulty,
    //     OsuScoreState score,
    //     uint mods
    // ) => Native.CalculateOsuPerformance(ref difficulty, ref score, mods);

    private struct PendingCalculation
    {
        public OsuJudgement Judgement;
        public ulong MaxCombo;
    }
}