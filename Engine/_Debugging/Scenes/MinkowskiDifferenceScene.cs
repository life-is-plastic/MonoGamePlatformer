using Microsoft.Xna.Framework;

namespace Engine.Debugging;

/// <summary>
/// See the Minkowski difference between two rectangles.
/// </summary>
public class MinkowskiDifferenceScene : BaseSceneDefinition
{
    public static MinkowskiDifferenceScene Instance { get; } = new();

    private MinkowskiDifferenceScene() { }

    public override void Initialize(Scene scene)
    {
        base.Initialize(scene);
        scene.Singletons.StageAttach(new Controller());
    }

    private class Controller : Component, IUpdatable
    {
        private static readonly Vector2 s_dims = new(32, 32);

        private Entity _a = null!;
        private Entity _b = null!;
        private Entity _ab_diff = null!;
        private Entity _ba_diff = null!;

        private Entity MakeCollider(Color color, Vector2 initialPosition)
        {
            return Scene
                .StageCreate("Object")
                .StageAttach(new Transform() { Position = initialPosition })
                .StageAttach(
                    new RectangleRenderer()
                    {
                        Size = s_dims,
                        NormalizedOrigin = new(0.5f),
                        Color = color * 0.5f,
                    }
                )
                .StageAttach(new Collider(s_dims) { NormalizedOrigin = new(0.5f) });
        }

        private Entity MakeDiffRepr(Color color)
        {
            return Scene
                .StageCreate("Object")
                .StageAttach(new Transform())
                .StageAttach(
                    new RectangleRenderer()
                    {
                        Size = s_dims,
                        NormalizedOrigin = new(0.5f),
                        Color = color * 0.25f,
                    }
                );
        }

        void IUpdatable.Update()
        {
            if (_a is null)
            {
                _a = MakeCollider(Color.Red, initialPosition: new(-50, -50));
                _b = MakeCollider(Color.Green, initialPosition: new(50, -50));
                _ab_diff = MakeDiffRepr(Color.Blue);
                _ba_diff = MakeDiffRepr(Color.Black);
                return;
            }

            var md = _a.Get<Collider>()
                .AsWorldRectangleF()
                .GetMinkowskiDifference(_b.Get<Collider>().AsWorldRectangleF());

            _ab_diff.Get<Transform>().Position = md.Center;
            _ab_diff.Get<RectangleRenderer>().Size = md.Size;
            _ab_diff.Get<RectangleRenderer>().NormalizedOrigin = new(0.5f);

            _ba_diff.Get<Transform>().Position = -md.Center;
            _ba_diff.Get<RectangleRenderer>().Size = md.Size;
            _ba_diff.Get<RectangleRenderer>().NormalizedOrigin = new(0.5f);
        }
    }
}
