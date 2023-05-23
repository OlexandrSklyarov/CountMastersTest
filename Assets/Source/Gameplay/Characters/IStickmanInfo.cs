using System;

namespace Source.Gameplay.Characters
{
    public interface IStickmanInfo
    {
        event Action<int, int> ChangeStickmanCountEvent;
    }
}