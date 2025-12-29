using System;

namespace Engine.Core;

/// <summary>
/// A half open interval <c>[Min, Max)</c>.
/// </summary>
public readonly struct UpdateOrderInterval
{
    public static UpdateOrderInterval FrameBegin { get; } = new(-1001, -1000);
    public static UpdateOrderInterval Physics { get; } = new(500, 600);
    public static UpdateOrderInterval FrameEnd { get; } = new(1000, 1001);

    public int Min { get; }
    public int Max { get; }
    public int Length => Max - Min;

    /// <summary>
    /// Converts a relative update order in this interval to an absolute update order.
    /// </summary>
    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }
            return Min + index;
        }
    }

    public UpdateOrderInterval(int a, int b)
    {
        Min = Math.Min(a, b);
        Max = Math.Max(a, b);
    }

    /// <summary>
    /// Returns a random value within this interval. The result is deterministic with respect to
    /// component type.
    /// </summary>
    public int GetRandom<T>()
        where T : IUpdatable
    {
        return new Random(typeof(T).GetHashCode()).Next(Min, Max);
    }
}
