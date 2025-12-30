using Engine.Core;

namespace Engine.Graphics;

public partial class AnimationRenderer<T> : DrawableRenderer<Animation<T>>
    where T : IDrawable
{
    public AnimationRenderer(Animation<T> animation)
        : base(animation) { }
}

public partial class AnimationRenderer<T> : IUpdatable
{
    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameBegin;

    void IUpdatable.Update()
    {
        if (IsVisible)
        {
            Drawable.Update(Scene.DeltaTime);
        }
    }
}
