using System;

namespace Engine;

/// <summary>
/// Interface for components that update every frame.
/// </summary>
public interface IUpdatable : IComponent
{
    // Preset update orders.
    public const int UpdateOrderFrameBegin = -1000;
    public const int UpdateOrderDefault = 0;
    public static UpdateOrderInterval UpdateOrderPhysics { get; } = new(500, 599);
    public const int UpdateOrderFrameEnd = 1000;

    /// <summary>
    /// Controls the order of <c>Update()</c> calls between component instances. All instances of
    /// the same concrete type should have the same update order; in other words, implement this
    /// like a class const and do not mutate it.
    /// </summary>
    public int UpdateOrder => UpdateOrderDefault;

    /// <summary>
    /// Invoked on scene pause. There is no guaranteed invocation order across components.
    /// </summary>
    /// <returns>True if this component's <c>Update()</c> should be blocked while paused.</returns>
    public bool Pause() => true;

    /// <summary>
    /// Invoked on scene resume. There is no guaranteed invocation order across components. This
    /// gets called regardless of <c>Pause()</c>'s return value.
    /// </summary>
    public void Unpause() { }

    /// <summary>
    /// Invoked every frame, or every unpaused frame if <c>Pause()</c> returns true.
    /// </summary>
    public void Update() { }

    /// <summary>
    /// A closed interval <c>[Min, Max]</c> of update orders.
    /// </summary>
    public readonly struct UpdateOrderInterval
    {
        public int Min { get; }
        public int Max { get; }
        public int Length => Max + 1 - Min;

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
            return new Random(typeof(T).GetHashCode()).Next(Min, Max + 1);
        }
    }
}
