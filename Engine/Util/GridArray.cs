using System.Diagnostics;

namespace Engine.Util;

/// <summary>
/// A row-major 2D array stored as a 1D array and indexable via row-column pairs.
/// </summary>
public readonly record struct GridArray<T>
{
    private readonly GridUtil _gridUtil;
    private readonly T[] _array;

    public int Rows => _gridUtil.Rows;
    public int Columns => _gridUtil.Columns;
    public int Length => _gridUtil.Count;

    public ref T this[int index]
    {
        get => ref _array[index];
    }
    public ref T this[int row, int column]
    {
        get => ref _array[_gridUtil[row, column]];
    }
    public ref T this[(int Row, int Column) rc]
    {
        get => ref this[rc.Row, rc.Column];
    }

    public GridArray(int rows, int columns)
        : this(rows, columns, new T[rows * columns]) { }

    /// <summary>
    /// Constructs using an existing array, which this grid array will take ownership over. The
    /// given array must be of the correct length.
    /// </summary>
    public GridArray(int rows, int columns, params T[] array)
    {
        _gridUtil = new GridUtil(rows, columns);
        Debug.Assert(array.Length == Length);
        _array = array;
    }

    public bool ContainsIndex(int row, int column)
    {
        return _gridUtil.ContainsIndex(row, column);
    }

    public bool ContainsIndex((int Row, int Column) rc)
    {
        return ContainsIndex(rc.Row, rc.Column);
    }
}
