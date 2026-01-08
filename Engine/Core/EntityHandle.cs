using System;

namespace Engine;

/// <summary>
/// Container for a cached entity reference. DO NOT store as a readonly field.
/// </summary>
public struct EntityHandle
{
    private Entity? _entity;
    private int _checkedFrame = int.MinValue;

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
        if (_entity is not null && _checkedFrame < _entity.Scene.FrameCount)
        {
            if (_entity.IsAlive())
            {
                _checkedFrame = _entity.Scene.FrameCount;
            }
            else
            {
                _entity = null;
            }
        }
        return _entity;
    }
}
