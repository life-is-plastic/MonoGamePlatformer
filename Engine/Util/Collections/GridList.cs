using System.Collections.Generic;

namespace Engine.Util.Collections;

public readonly record struct GridList<T>
{
    private readonly Grid _grid;
    private readonly List<T> _list;

    public int Rows => _grid.Rows;
    public int Columns => _grid.Columns;
    public int Count => _grid.Count;

    public GridList(int columns, List<T> list) { }
}
