using System;
using System.Diagnostics;

namespace Engine.Util;

/// <summary>
/// Converts between 1D coordinates and row-major 2D coordinates.
/// </summary>
public readonly record struct GridUtil
{
    public int Rows { get; }
    public int Columns { get; }
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
            Debug.Assert(ContainsIndex(row, column));
            return row * Columns + column;
        }
    }

    public int this[(int Row, int Column) rc] => this[rc.Row, rc.Column];

    public GridUtil(int rows, int columns)
    {
        Debug.Assert(rows > 0);
        Debug.Assert(columns > 0);
        Rows = rows;
        Columns = columns;
    }

    public bool ContainsIndex(int row, int column)
    {
        return 0 <= row && row < Rows && 0 <= column && column < Columns;
    }

    public bool ContainsIndex((int Row, int Column) rc)
    {
        return ContainsIndex(rc.Row, rc.Column);
    }
}
