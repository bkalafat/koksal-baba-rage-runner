using UnityEngine;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Moving barrier obstacle that oscillates vertically.
    /// </summary>
    public class MovingBarrier : Obstacle
    {
        [SerializeField] private float amplitude = 2.0f;
        [SerializeField] private float frequency = 1.0f;
        private float _startY;
        private float _time;

        private void Start()
        {
            _startY = transform.position.y;
        }

        public override void Initialize(float speed)
        {
            base.Initialize(speed);
            _time = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (!_isActive) return;

            // Oscillate vertically
            _time += Time.deltaTime;
            float newY = _startY + Mathf.Sin(_time * frequency * Mathf.PI * 2) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        public override void Reset()
        {
            base.Reset();
            _time = 0f;
            transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
        }
    }
}
