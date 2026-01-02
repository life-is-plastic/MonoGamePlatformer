namespace Engine;

public class AnimationRenderer<T> : DrawableRenderer<Animation<T>>, IUpdatable
    where T : IDrawable
{
    public AnimationRenderer(Animation<T> animation)
        : base(animation) { }

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameBegin;

    void IUpdatable.Update()
    {
        if (IsVisible)
        {
            Drawable.Update(Scene.DeltaTime);
        }
    }
}
