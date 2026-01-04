using System;
using System.Diagnostics;

namespace Engine.Util;

/// <summary>
/// Models an arbitrary row-major 2D grid.
/// </summary>
public readonly record struct GridHelper
{
    public required int Rows
    {
        get;
        init
        {
            Debug.Assert(value >= 0);
            field = value;
        }
    }

    public required int Columns
    {
        get;
        init
        {
            Debug.Assert(value >= 0);
            field = value;
        }
    }

    public int Count => Rows * Columns;

    /// <summary>
    /// Converts an index to a row-column pair.
    /// </summary>
    public (int Row, int Column) this[int index]
    {
        get
        {
            Debug.Assert(index >= 0 && index < Count);
            var r = Math.DivRem(index, Columns, out var c);
            return (r, c);
        }
    }

    /// <summary>
    /// Converts a row-column pair to an index.
    /// </summary>
    public int this[int row, int column]
    {
        get
        {
            Debug.Assert(row >= 0 && row < Rows);
            Debug.Assert(column >= 0 && column < Columns);
            return row * Columns + column;
        }
    }

    public int this[(int Row, int Column) rc] => this[rc.Row, rc.Column];
}
