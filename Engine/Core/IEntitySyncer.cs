namespace Engine.Core;

/// <summary>
/// Interface for components interested in entity create/destroy or component attach/detach events.
/// </summary>
public interface IEntitySyncer : IComponent
{
    public void Sync(EntityChangelist entityChangelist);
}
