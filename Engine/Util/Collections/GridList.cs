using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine.Util.Collections;

/// <summary>
/// A row-major 2D matrix stored as a 1D list and indexable via row-column pairs.
/// </summary>
public readonly record struct GridList<T>
{
    private readonly Grid _grid;
    private readonly List<T> _list;

    public int Rows => _grid.Rows;
    public int Columns => _grid.Columns;
    public int Count => _grid.Count;

    public ref T this[int index] => ref AsSpan()[index];
    public ref T this[int row, int column] => ref AsSpan()[_grid[row, column]];
    public ref T this[(int Row, int Column) rc] => ref this[rc.Row, rc.Column];

    public GridList(int columns)
        : this(columns, new()) { }

    /// <summary>
    /// Constructs using an existing list, which this grid list will take ownership over. If needed,
    /// the given list will be expanded to a count that is a multiple of <c>columns</c>, with the
    /// default element value filling the expanded slots.
    /// </summary>
    public GridList(int columns, List<T> list)
    {
        while (list.Count % Columns != 0)
        {
            list.Add(default!);
        }
        _grid = new(list.Count / Columns, columns);
        _list = list;
    }

    public Span<T> AsSpan()
    {
        return CollectionsMarshal.AsSpan(_list);
    }

    public Span<T> GetRow(int row)
    {
        return AsSpan().Slice(_grid[row, 0], Columns);
    }

    public void AddRow()
    {
        for (var c = 0; c < Columns; c++)
        {
            _list.Add(default!);
        }
    }
}
