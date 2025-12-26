using Microsoft.Xna.Framework;

namespace Engine.Util.Extensions;

public static class RectangleExtenions
{
    extension(Rectangle rect)
    {
        public RectangleF ToRectangleF()
        {
            return new(rect.Location.ToVector2(), rect.Size.ToVector2());
        }
    }
}
