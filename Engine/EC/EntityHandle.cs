using System;

namespace Engine.EC;

/// <summary>
/// Container for a cached entity reference.
/// </summary>
public readonly struct EntityHandle
{
    private readonly Entity _entity;

    public EntityHandle(Entity entity)
    {
        _entity = entity;
    }

    /// <summary>
    /// Returns the wrapped entity, throwing an exception if the entity is no longer alive.
    /// </summary>
    public Entity Deref()
    {
        return MaybeDeref() ?? throw new InvalidOperationException($"{_entity} is no longer alive");
    }

    /// <summary>
    /// Returns the wrapped entity, or null if the entity is no longer alive.
    /// </summary>
    public Entity? MaybeDeref()
    {
        return _entity.IsAlive() ? _entity : null;
    }
}
