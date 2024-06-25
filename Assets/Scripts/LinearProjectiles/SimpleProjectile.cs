using UnityEngine;

namespace LinearProjectiles
{
    public class SimpleProjectile : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Vector3 _endPosition;

        /// <summary>
        /// Shoots the projectile to the end position.
        /// </summary>
        /// <param name="endPosition"> The position where the projectile should go. </param>
        public void Shoot(Vector3 endPosition)
        {
            _endPosition = endPosition;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _endPosition,
                speed * Time.fixedDeltaTime
            );

            if (transform.position == _endPosition)
            {
                gameObject.SetActive(false);
            }
        }
    }
}