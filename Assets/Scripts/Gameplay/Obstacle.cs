using UnityEngine;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Base class for obstacles that the player must avoid or dash through.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] protected float scrollSpeed = 6.0f;
        protected bool _isActive = false;

        public virtual void Initialize(float speed)
        {
            scrollSpeed = speed;
            _isActive = true;
        }

        protected virtual void Update()
        {
            if (!_isActive) return;

            // Scroll leftward
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // Check if off-screen (return to pool)
            if (transform.position.x < -10.0f)
            {
                ReturnToPool();
            }
        }

        protected virtual void ReturnToPool()
        {
            _isActive = false;
            gameObject.SetActive(false);
            // Actual pool return handled by Spawner
        }

        public virtual void Reset()
        {
            _isActive = false;
        }
    }
}
