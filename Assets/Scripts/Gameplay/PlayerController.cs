using UnityEngine;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Controls player movement, collision detection, and dash state.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] private float forwardSpeed = 5.0f;
        [SerializeField] private float jumpSpeed = 8.0f;

        [Header("Boundaries")]
        [SerializeField] private float upperBoundY = 5.0f;
        [SerializeField] private float lowerBoundY = -3.0f;

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private bool _isDashing = false;
        private bool _isInvulnerable = false;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _rb.gravityScale = 2.5f;
        }

        private void Update()
        {
            // Check for tap input via IInputService
            if (Input.GetMouseButtonDown(0) && Core.GameManager.Instance.CurrentState == Core.GameManager.GameState.Playing)
            {
                ApplyHopImpulse();
            }

            // Clamp Y position
            if (transform.position.y > upperBoundY)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
                transform.position = new Vector3(transform.position.x, upperBoundY, transform.position.z);
            }
            else if (transform.position.y < lowerBoundY)
            {
                TriggerGameOver("Fell off world");
            }
        }

        private void ApplyHopImpulse()
        {
            _rb.linearVelocity = new Vector2(forwardSpeed, jumpSpeed);
            // TODO: Play hop SFX and trigger light haptic
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                if (!_isDashing && !_isInvulnerable)
                {
                    TriggerGameOver(other.gameObject.name);
                }
                else if (_isDashing && other.GetComponent<BreakableCrate>() != null)
                {
                    // Break crate and award score
                    other.GetComponent<BreakableCrate>().Break();
                    // TODO: Award +10 score
                }
            }
            else if (other.CompareTag("Pickup"))
            {
                OnPickupCollected(other.gameObject);
            }
        }

        private void OnPickupCollected(GameObject pickup)
        {
            // TODO: Determine pickup type (Taunt, Coin), award rage/coins/score, play SFX, return to pool
            Debug.Log($"Collected pickup: {pickup.name}");
        }

        private void TriggerGameOver(string cause)
        {
            Debug.Log($"Game Over: {cause}");
            // TODO: Play death SFX, trigger error haptic, transition to GameOver state
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.GameOver);
        }

        public void StartDash(float duration, float speedMultiplier)
        {
            _isDashing = true;
            forwardSpeed *= speedMultiplier;
            // TODO: Enable red sprite tint, enable particle trail, play rage SFX
            Invoke(nameof(EndDash), duration);
        }

        private void EndDash()
        {
            _isDashing = false;
            forwardSpeed /= 1.3f; // Restore normal speed
            // TODO: Disable sprite tint, disable particle trail, stop rage SFX
        }

        public void Respawn(Vector3 position)
        {
            transform.position = position;
            _rb.linearVelocity = new Vector2(forwardSpeed, 0);
            _isInvulnerable = true;
            Invoke(nameof(EndInvulnerability), 1.0f);
            // TODO: Enable flashing sprite effect
        }

        private void EndInvulnerability()
        {
            _isInvulnerable = false;
            // TODO: Disable flashing sprite effect
        }
    }
}
