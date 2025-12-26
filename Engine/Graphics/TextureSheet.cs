using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public readonly record struct TextureSheet
{
    private readonly GridUtil _gridUtil;

    public TextureRegion TextureRegion { get; }
    public int Rows => _gridUtil.Rows;
    public int Columns => _gridUtil.Columns;
    public int FrameCount => _gridUtil.Count;
    public int FrameWidth => TextureRegion.Region.Width / _gridUtil.Columns;
    public int FrameHeight => TextureRegion.Region.Height / _gridUtil.Rows;
    public Point FrameSize => new(FrameWidth, FrameHeight);

    /// <summary>
    /// Gets the frame at the given index.
    /// </summary>
    public TextureRegion this[int index] => this[_gridUtil[index]];

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
        _gridUtil = new(rows, columns);
        TextureRegion = textureRegion;
    }
}
