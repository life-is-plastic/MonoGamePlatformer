using System;
using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Microsoft.Xna.Framework;

namespace Engine.Util.Debugging;

public partial class ColliderMouseDrag : Component
{
    private readonly Func<Entity, bool> _entityFilter;
    private readonly Button _button;
    private EntityHandle _cameraHandle;
    private Entity? _dragged = null;
    private Point _initialScreenPos = default;
    private Vector2 _initialWorldPos = default;

    public ColliderMouseDrag(Func<Entity, bool> entityFilter, Button? button = null)
    {
        _entityFilter = entityFilter;
        _button = button ?? MouseButton.Left;
    }

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class ColliderMouseDrag : IUpdatable
{
    bool IUpdatable.Pause()
    {
        _dragged = null;
        return true;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();

        if (_dragged is null)
        {
            if (inputManager.IsPressed(_button))
            {
                foreach (var entity in Scene.Entities)
                {
                    var collider = entity.MaybeGet<Collider>();
                    if (collider is null)
                    {
                        continue;
                    }
                    if (!_entityFilter(entity))
                    {
                        continue;
                    }
                    if (!collider.AsWorldRectangleF().Contains(inputManager.MouseWorldPosition))
                    {
                        continue;
                    }

                    _dragged = entity;
                    _initialWorldPos = _dragged.Get<Transform>().Position;
                    _initialScreenPos = inputManager.MouseScreenPosition;
                    break;
                }
            }
            return;
        }

        if (!inputManager.IsDown(_button))
        {
            _dragged = null;
            return;
        }

        var deltaScreenPos =
            inputManager.MouseScreenPosition.ToVector2() - _initialScreenPos.ToVector2();
        var deltaWorldPos = deltaScreenPos / (camera.ScreenScale * cameraTransform.Scale);
        _dragged.Get<Transform>().Position = _initialWorldPos + deltaWorldPos;
    }
}
