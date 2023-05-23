using UnityEngine;

public interface IGamepad
{
    public event System.Action<Vector2> StickInputEvent;

    void Activate();
    void Deactivate();
}