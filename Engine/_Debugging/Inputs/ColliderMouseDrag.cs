using System;
using Microsoft.Xna.Framework;

namespace Engine.Debugging;

public class ColliderMouseDrag : Component, IUpdatable
{
    private EntityHandle _cameraHandle;
    private Entity? _dragged = null;
    private Point _initialScreenPos = new();
    private Vector2 _initialWorldPos = new();

    public Func<Entity, bool> EntityFilter { get; init; } = entity => true;
    public Button Button { get; init; } = MouseButton.Left;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

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
            if (inputManager.IsPressed(Button))
            {
                foreach (var entity in Scene.Find<Collider>())
                {
                    var collider = entity.Get<Collider>();
                    if (!EntityFilter(entity))
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

        if (!inputManager.IsDown(Button))
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
