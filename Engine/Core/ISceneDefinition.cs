namespace Engine.Core;

public interface ISceneDefinition
{
    public string Name();

    /// <summary>
    /// Calling this is the scene constructor's final action. Initial scene entities should be
    /// created here.
    /// </summary>
    public void Initialize(Scene scene);
}
