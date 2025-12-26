using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public readonly record struct TextureSheet
{
    private readonly GridHelper _gridHelper;

    public TextureRegion TextureRegion { get; }
    public int Rows => _gridHelper.Rows;
    public int Columns => _gridHelper.Columns;
    public int FrameCount => _gridHelper.Count;
    public int FrameWidth => TextureRegion.Region.Width / _gridHelper.Columns;
    public int FrameHeight => TextureRegion.Region.Height / _gridHelper.Rows;
    public Point FrameSize => new(FrameWidth, FrameHeight);

    /// <summary>
    /// Gets the frame at the given index.
    /// </summary>
    public TextureRegion this[int index] => this[_gridHelper[index]];

    /// <summary>
    /// Gets the frame at the given row-column pair.
    /// </summary>
    public TextureRegion this[(int Row, int Column) rc] =>
        new(
            TextureRegion,
            new Rectangle(rc.Column * FrameWidth, rc.Row * FrameHeight, FrameWidth, FrameHeight)
        );

    public TextureSheet(in TextureRegion textureRegion, int rows, int columns)
    {
        _gridHelper = new(rows, columns);
        TextureRegion = textureRegion;
    }
}
