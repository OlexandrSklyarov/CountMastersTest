using Source.Data;
using TMPro;
using UnityEngine;

namespace Source.Gameplay.Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class StairBlock : MonoBehaviour, IFinishBlock
    {
        int IFinishBlock.Points => _points;

        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private TextMeshPro _pointView;

        private int _points;



        public void Init(int points, FinishStairsData.StairColor color) 
        {
            _points = points;

            _renderer.material.SetColor("_BaseColor", color.Base);
            _renderer.material.SetColor("_EmissionColor", color.Emission);

            _pointView.text = $"x{_points}";
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}