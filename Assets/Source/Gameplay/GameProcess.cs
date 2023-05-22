
using System;
using Source.Gameplay.Environment;

namespace Source.Gameplay
{
    public class GameProcess
    {
        private readonly IRoad _road;

        public event Action CompletedEvent;            
        public event Action FailureEvent;           


        public GameProcess(IRoad road)
        {
            _road = road;
        }


        public void OnUpdate()
        {
            
        }


        public void Play()
        {
            
        }


        public void Stop()
        {
            
        }
    }
}