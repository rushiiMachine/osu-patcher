using System;
using System.Collections.Concurrent;
using System.Threading;
using JetBrains.Annotations;

namespace Osu.Performance.ROsu;

[UsedImplicitly]
public class OsuPerformance : IDisposable
{
    private readonly Thread _calculatingThread;
    private readonly ConcurrentQueue<PendingCalculation> _queue;
    private readonly IntPtr _state;
    private volatile int _closed;

    public OsuPerformance(string mapPath, uint mods)
    {
        _state = Native.InitializeOsuGradualPerformance(mapPath, mods);
        _queue = new ConcurrentQueue<PendingCalculation>();
        _closed = 0;

        _calculatingThread = new Thread(ProcessQueue)
        {
            IsBackground = true,
        };

        _calculatingThread.Start();
    }

    public void Dispose()
    {
        Interlocked.Exchange(ref _closed, 1);
        OnNewCalculation = null;
        _calculatingThread.Abort();
        Native.DisposeGradualOsuPerformance(_state);
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
        if (_closed > 0) return;

        _queue.Enqueue(new PendingCalculation
        {
            Judgement = judgement,
            MaxCombo = maxCombo,
        });
    }

    private void ProcessQueue()
    {
        while (true)
        {
            if (_closed > 0) return;

            while (_queue.TryDequeue(out var item))
            {
                var performance = Native.CalculateGradualOsuPerformance(_state, item.Judgement, item.MaxCombo);
                var clamped = Math.Max(0, performance);

                OnNewCalculation?.Invoke(clamped);

                if (_closed > 0) return;
            }

            Thread.Sleep(100);
        }
    }

    /// <summary>
    ///     Calculates the performance metrics of a score while and returns the complete info.
    ///     If this is a failed score, or is in progress for whatever reason, then the end of the score will be
    ///     calculated based on the sum of the amount of hits recorded in <paramref name="score" />.
    /// </summary>
    /// <param name="difficulty">The precalculated/cached difficulty attributes of a map.</param>
    /// <param name="score">A completed (or failed) score's info on the associated map.</param>
    /// <param name="mods">The set of mods that were used on this score.</param>
    [UsedImplicitly]
    public static OsuPerformanceInfo CalculateScore(
        OsuDifficultyAttributes difficulty,
        OsuScoreState score,
        uint mods
    ) => Native.CalculateOsuPerformance(ref difficulty, ref score, mods);

    private struct PendingCalculation
    {
        public OsuJudgement Judgement;
        public ulong MaxCombo;
    }
}