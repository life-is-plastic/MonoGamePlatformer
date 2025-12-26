using System.Collections.Generic;
using System.Diagnostics;
using Engine.Util;
using Engine.Util.Extensions;

namespace Engine.Core;

public class EntityUpdater
{
    private static readonly Comparer<IUpdatable> s_updateOrderComparer =
        Comparer<IUpdatable>.Create(
            (a, b) =>
                (a.UpdateOrder, a.GetType().GetHashCode()).CompareTo(
                    (b.UpdateOrder, b.GetType().GetHashCode())
                )
        );

    private readonly IndexedSet<IUpdatable> _active = new();
    private readonly HashSet<IUpdatable> _paused = new();

    public bool IsPaused { get; private set; } = false;

    public void Add(IUpdatable updatable)
    {
        if (IsPaused && updatable.Pause())
        {
            Debug.Assert(!_active.Contains(updatable));
            _paused.AddOrDie(updatable);
        }
        else
        {
            Debug.Assert(!_paused.Contains(updatable));
            _active.AddOrDie(updatable);
        }
    }

    public void Remove(IUpdatable updatable)
    {
        var removedActive = _active.Remove(updatable);
        var removedPaused = _paused.Remove(updatable);
        Debug.Assert(removedActive ^ removedPaused);
    }

    public void ProcessPausing(bool shouldPause)
    {
        if (shouldPause == IsPaused)
        {
            return;
        }

        if (shouldPause)
        {
            foreach (var updatable in _active)
            {
                if (updatable.Pause())
                {
                    _paused.AddOrDie(updatable);
                }
            }
            foreach (var updatable in _paused)
            {
                _active.RemoveOrDie(updatable);
            }
        }
        else
        {
            foreach (var updatable in _active)
            {
                updatable.Unpause();
            }
            foreach (var updatable in _paused)
            {
                updatable.Unpause();
                _active.AddOrDie(updatable);
            }
            _paused.Clear();
        }
        IsPaused = shouldPause;
    }

    public void Update()
    {
        _active.Sort(s_updateOrderComparer);
        foreach (var updatable in _active)
        {
            updatable.Update();
        }
    }
}
