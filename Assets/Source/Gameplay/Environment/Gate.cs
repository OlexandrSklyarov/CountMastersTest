using Source.Gameplay.Characters;
using TMPro;
using UnityEngine;
using System;

namespace Source.Gameplay.Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class Gate : MonoBehaviour, IInteractable
    {
        private enum GateType {ADDER, MULTIPLIER}

        [SerializeField] private GateType _type;
        [SerializeField] private TextMeshPro _view;

        private int rndNum;

        public event Action HitEvent;


        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            SetRandomNumber();
        }


        private void SetRandomNumber()
        {
            switch (_type)
            {
                case GateType.ADDER:

                    rndNum = UnityEngine.Random.Range(10, 100);
                    RoundNumber();
                    _view.text = $"+{rndNum}";

                    break;

                case GateType.MULTIPLIER:

                    rndNum = UnityEngine.Random.Range(1, 3);
                    RoundNumber();
                    _view.text = $"X{rndNum}";

                    break;
            }
        }


        private void RoundNumber()
        {
            if (rndNum % 2 != 0) rndNum += 1;
        }
        

        void IInteractable.Interact(IInteractTarget target)
        {
            switch(_type)
            {
                case GateType.ADDER:

                    target.AddStickman(rndNum);

                    break;

                case GateType.MULTIPLIER:

                    target.MultiplyStickman(rndNum);
                    
                    break;
            }

            HitEvent?.Invoke();
        }
    }
}