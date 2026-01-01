using System;
using System.Diagnostics;

namespace Engine.Util;

/// <summary>
/// A row-major 2D matrix stored as a 1D array and indexable via row-column pairs.
/// </summary>
public readonly record struct GridArray<T>
{
    private readonly GridHelper _gridHelper;
    private readonly T[] _array;

    public int Rows => _gridHelper.Rows;
    public int Columns => _gridHelper.Columns;
    public int Length => _gridHelper.Count;

    public ref T this[int index] => ref _array[index];
    public ref T this[int row, int column] => ref _array[_gridHelper[row, column]];
    public ref T this[(int Row, int Column) rc] => ref this[rc.Row, rc.Column];

    public GridArray()
        : this(0, 0) { }

    public GridArray(int rows, int columns)
        : this(rows, columns, rows * columns == 0 ? Array.Empty<T>() : new T[rows * columns]) { }

    /// <summary>
    /// Constructs using an existing array, which this grid array will take ownership over. The
    /// given array must be of the correct length.
    /// </summary>
    public GridArray(int rows, int columns, params T[] array)
    {
        Debug.Assert(rows * columns == array.Length);
        _gridHelper = new GridHelper(rows, columns);
        _array = array;
    }

    public Span<T> AsSpan()
    {
        return _array;
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        ReadOnlySpan<T> span = AsSpan();
        return span.GetEnumerator();
    }
}
