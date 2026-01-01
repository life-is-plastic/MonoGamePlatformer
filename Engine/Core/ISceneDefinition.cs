namespace Engine;

public interface ISceneDefinition
{
    /// <summary>
    /// Defines the scene's name.
    /// </summary>
    public string Name();

    /// <summary>
    /// Calling this is the scene constructor's final action. Initial scene entities should be
    /// created here.
    /// </summary>
    public void Initialize(Scene scene);
}
