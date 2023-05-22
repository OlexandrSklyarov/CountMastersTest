using System;

namespace Source.Gameplay.Characters
{
    public interface IStickmanChange
    {
        event Action<int, int> ChangeStickmanCountEvent;
    }
}