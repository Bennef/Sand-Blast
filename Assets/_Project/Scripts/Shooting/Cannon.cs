using Scripts.Audio;
using UnityEngine;

namespace Scripts.Shooting
{
    /// <summary>
    /// Handles the firing of cannonballs.
    /// Credit to Jason Weimann for help with the basics of this script.
    /// </summary>
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private float _force = 35f;
        [SerializeField] private Transform _cannonball;
        private SFXManager _sFXManager;

        void Start() => _sFXManager = FindObjectOfType<SFXManager>();

        /// <summary>
        /// Fire at point of click.
        /// </summary>
        public void Fire()
        {
            _sFXManager.PlaySound(_sFXManager.Fire);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var velocity = BallisticVelocity(hitInfo.point, _force);
                var ball = Instantiate(_cannonball, transform.position, Quaternion.identity);
                ball.GetComponent<Rigidbody>().velocity = velocity;
            }
        }

        /// <summary>
        /// Calculate the required velocity for the cannonball.
        /// </summary>
        /// <param name="destination">Destination will be the hit point of the raycast.</param>
        /// <param name="force">Force of the cannonball.</param>
        /// <returns>Normalised vector of velocity for ball</returns>
        private Vector3 BallisticVelocity(Vector3 destination, float force)
        {
            Vector3 dir = destination - transform.position; // get Target Direction
            return dir.normalized * force; // Return a normalized vector.
        }
    }
}