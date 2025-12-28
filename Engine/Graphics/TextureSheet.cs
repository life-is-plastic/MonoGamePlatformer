using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public readonly record struct TextureSheet
{
    private readonly Grid _grid;

    public TextureRegion TextureRegion { get; }
    public int Rows => _grid.Rows;
    public int Columns => _grid.Columns;
    public int FrameCount => _grid.Count;
    public int FrameWidth => TextureRegion.Region.Width / _grid.Columns;
    public int FrameHeight => TextureRegion.Region.Height / _grid.Rows;
    public Point FrameSize => new(FrameWidth, FrameHeight);

    /// <summary>
    /// Gets the frame at the given index.
    /// </summary>
    public TextureRegion this[int index] => this[_grid[index]];

    /// <summary>
    /// Gets the frame at the given row-column pair.
    /// </summary>
    public TextureRegion this[(int Row, int Column) rc] =>
        new(
            TextureRegion,
            new Rectangle(rc.Column * FrameWidth, rc.Row * FrameHeight, FrameWidth, FrameHeight)
        );

    public TextureSheet(TextureRegion textureRegion, int rows, int columns)
    {
        _grid = new(rows, columns);
        TextureRegion = textureRegion;
    }
}
