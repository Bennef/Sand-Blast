using Scripts.Audio;
using System.Collections.Generic;
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
        [SerializeField] private GameObject _cannonballPrefab;
        [SerializeField] int _cannonballCount = 10;
        [SerializeField] private List<GameObject> _ballPool;
        [SerializeField] int _cannonballIndex = 0;
        SFXManager _sFXManager;

        void Start()
        {
            _sFXManager = FindObjectOfType<SFXManager>();
            for (int i = 0; i < _cannonballCount; i++)
            {
                _ballPool.Add(_cannonballPrefab);
                _ballPool[i].gameObject.SetActive(false);
                Instantiate(_ballPool[i], transform.position, Quaternion.identity);
            }
        }

        /// <summary>
        /// Fire at point of click.
        /// </summary>
        public void Fire()
        {
            _sFXManager.PlaySound(_sFXManager.Fire);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                //var ball = Instantiate(_ballPool[_cannonballIndex], transform.position, Quaternion.identity);
                GameObject ball = GetBall();
                if (ball != null)
                {
                    ball.SetActive(true);
                    print(ball.GetInstanceID().ToString());
                    print(ball.activeInHierarchy);
                    var velocity = BallisticVelocity(hitInfo.point, _force);
                    ball.GetComponent<Rigidbody>().velocity = velocity;
                    _cannonballIndex++;
                }
            }
        }

        private GameObject GetBall()
        {
            return _ballPool[_cannonballIndex];
        }

        /// <summary>
        /// Calculate the required velocity for the cannonball.
        /// </summary>
        /// <param name="destination">Destination will be the hit point of the raycast.</param>
        /// <param name="force">Force of the cannonball.</param>
        /// <returns>Normalised vector of velocity for ball</returns>
        private Vector3 BallisticVelocity(Vector3 destination, float force)
        {
            Vector3 dir = destination - transform.position; // get target Direction
            return dir.normalized * force; // return a normalized vector.
        }
    }
}