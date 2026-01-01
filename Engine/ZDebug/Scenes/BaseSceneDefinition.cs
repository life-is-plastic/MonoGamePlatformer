using Engine.Core;
using Engine.Graphics;
using Engine.ZDebug.Inputs;
using Engine.ZDebug.Visuals;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Scenes;

public abstract class BaseSceneDefinition : ISceneDefinition
{
    public virtual string Name()
    {
        return GetType().Name;
    }

    public virtual void Initialize(Scene scene)
    {
        scene
            .Singletons.StageAttach(new SceneLoadOnPress(this))
            .StageAttach(new ScenePauseToggle())
            .StageAttach(new CameraMouseZoom())
            .StageAttach(new CameraMouseDrag())
            .StageAttach(new ColliderMouseDrag())
            .StageAttach(new DrawHelper())
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new OriginRenderer());
    }
}
