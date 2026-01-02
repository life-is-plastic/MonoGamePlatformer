using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine;

public readonly record struct TextureSheet
{
    private readonly GridHelper _gridHelper;

    public TextureRegion TextureRegion { get; }
    public int Rows => _gridHelper.Rows;
    public int Columns => _gridHelper.Columns;
    public int TileCount => _gridHelper.Count;
    public int TileWidth => TextureRegion.Region.Width / _gridHelper.Columns;
    public int TileHeight => TextureRegion.Region.Height / _gridHelper.Rows;
    public Point TileSIze => new(TileWidth, TileHeight);

    /// <summary>
    /// Gets the tile at the given index.
    /// </summary>
    public TextureRegion this[int index] => this[_gridHelper[index]];

    /// <summary>
    /// Gets the tile at the given row-column pair.
    /// </summary>
    public TextureRegion this[(int Row, int Column) rc] =>
        new(
            TextureRegion,
            new Rectangle(rc.Column * TileWidth, rc.Row * TileHeight, TileWidth, TileHeight)
        );

    public TextureSheet(in TextureRegion textureRegion, int rows, int columns)
    {
        _gridHelper = new(rows, columns);
        TextureRegion = textureRegion;
    }
}
