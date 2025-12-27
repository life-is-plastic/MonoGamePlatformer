using System.Diagnostics;

namespace Engine.Util.Collections;

/// <summary>
/// A row-major 2D array stored as a 1D array and indexable via row-column pairs.
/// </summary>
public readonly record struct GridArray<T>
{
    private readonly Grid _grid;
    private readonly T[] _array;

    public int Rows => _grid.Rows;
    public int Columns => _grid.Columns;
    public int Length => _grid.Count;

    public ref T this[int index] => ref _array[index];
    public ref T this[int row, int column] => ref _array[_grid[row, column]];
    public ref T this[(int Row, int Column) rc] => ref this[rc.Row, rc.Column];

    public GridArray(int rows, int columns)
        : this(rows, columns, new T[rows * columns]) { }

    /// <summary>
    /// Constructs using an existing array, which this grid array will take ownership over. The
    /// given array must be of the correct length.
    /// </summary>
    public GridArray(int rows, int columns, params T[] array)
    {
        _grid = new Grid(rows, columns);
        Debug.Assert(array.Length == Length);
        _array = array;
    }
}
