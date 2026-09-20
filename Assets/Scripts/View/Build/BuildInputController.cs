using Game.Core;
using UnityEngine;

namespace Game.Views
{
    public class BuildInputController : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private string towerConfigId = "basic_tower";

        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }

        private void Update()
        {
            if (gameLoop == null || gameLoop.Context == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                var cam = Camera.main;
                if (cam == null) return;

                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 1000f, groundLayer))
                {
                    gameLoop.Context.BuildSystem.TryBuild(hit.point, towerConfigId);
                }
            }
        }
    }
}