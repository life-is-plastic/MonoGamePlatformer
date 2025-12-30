using Engine.Core;
using Microsoft.Xna.Framework.Input;

namespace Engine.Util.Debugging;

public class PhysicsSandboxScene : ISceneDefinition
{
    public static PhysicsSandboxScene Instance { get; } = new();

    private PhysicsSandboxScene() { }

    string ISceneDefinition.Name()
    {
        return nameof(PhysicsSandboxScene);
    }

    void ISceneDefinition.Initialize(Scene scene)
    {
        scene
            .Singletons.StageAttach(new Controller())
            .StageAttach(new SceneLoadOnPress(Instance))
            .StageAttach(new ScenePauseToggle())
            .StageAttach(new CameraMouseZoom())
            .StageAttach(new CameraMouseDrag())
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new DrawHelper());
    }

    private class Controller : Component, IUpdatable
    {
        void IUpdatable.Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
