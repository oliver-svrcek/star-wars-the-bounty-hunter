using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameEndingCreditsAnimationManagement : MonoBehaviour
    {
        private GameEndingCreditsManagement GameEndingCreditsManagement { get; set; }

        private void Awake()
        {
            GameEndingCreditsManagement = Utils.GetComponentOrThrow<GameEndingCreditsManagement>("Interface/MainCamera/UICanvas/GameEndingCredits");
        }

        private void EndGame()
        {
            GameEndingCreditsManagement.EndGame();
        }
    }
}