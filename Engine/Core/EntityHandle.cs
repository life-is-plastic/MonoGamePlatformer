using System;

namespace Engine.Core;

/// <summary>
/// Store cached entity references inside this struct.
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
        return _entity.IsAlive()
            ? _entity
            : throw new InvalidOperationException($"{_entity} is no longer alive");
    }

    /// <summary>
    /// Returns the wrapped entity, or null if the entity is no longer alive.
    /// </summary>
    public Entity? MaybeDeref()
    {
        return _entity.IsAlive() ? _entity : null;
    }
}
