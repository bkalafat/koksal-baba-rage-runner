using UnityEngine;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Breakable crate obstacle that can be destroyed during rage dash.
    /// Awards +10 score bonus when broken.
    /// </summary>
    public class BreakableCrate : Obstacle
    {
        [SerializeField] private GameObject breakEffectPrefab;

        public void Break()
        {
            // TODO: Spawn break particle effect
            // TODO: Play break SFX
            // TODO: Award +10 score via ScoreService
            
            Debug.Log("Crate broken!");
            ReturnToPool();
        }

        public override void Reset()
        {
            base.Reset();
            // Reset crate sprite to intact state
        }
    }
}
