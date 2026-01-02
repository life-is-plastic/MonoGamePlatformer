using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Engine.Util;

namespace Engine;

/// <summary>
/// Defines collision layer relationships. By default:
/// <list type="bullet">
/// <item>Every layer collides with every other layer.</item>
/// <item>Every layer except <c>StaticGeometry</c> collides with itself.</item>
/// </list>
/// </summary>
public class CollisionLayers
{
    /// <summary>
    /// Number of supported layers. Layer values range from 0 to 1 less than this number.
    /// </summary>
    public static int Count => 8;

    /// <summary>
    /// The predefined default layer.
    /// </summary>
    public static int Default => 0;

    /// <summary>
    /// The predefined layer for static level geometry.
    /// </summary>
    public static int StaticGeometry => 7;

    private InlineArray8<byte> _layers = new();

    public CollisionLayers()
    {
        Span<byte> span = _layers;
        span.Fill(byte.MaxValue);
        SetCollidable(StaticGeometry, StaticGeometry, false);
    }

    public bool IsCollidable(int layer1, int layer2)
    {
        Debug.Assert(layer1 >= 0 && layer1 < _layers.Length);
        Debug.Assert(layer2 >= 0 && layer2 < _layers.Length);
        return (_layers[layer1] & 1 << layer2) != 0;
    }

    public void SetCollidable(int layer1, int layer2, bool collidable)
    {
        Debug.Assert(layer1 >= 0 && layer1 < _layers.Length);
        Debug.Assert(layer2 >= 0 && layer2 < _layers.Length);
        if (collidable)
        {
            _layers[layer1] |= (byte)(1 << layer2);
            _layers[layer2] |= (byte)(1 << layer1);
        }
        else
        {
            _layers[layer1] &= (byte)~(1 << layer2);
            _layers[layer2] &= (byte)~(1 << layer1);
        }
    }
}
