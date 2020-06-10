using UnityEngine;

namespace SandBlast
{
    public class Block : MonoBehaviour
    {
        public bool notClear = true;

        private float clearedDistance = 10f;

        private Vector3 startPosition;
        

        /// <summary>
        /// Set up references.
        /// </summary>
        void Start()
        {
            startPosition = gameObject.transform.position;
        }

        /// <summary>
        /// If the blocks fall below the platform level or are a given distance away, they are cleared.
        /// </summary>
        void Update()
        {
            float distanceFromPlatform = Vector3.Distance(gameObject.transform.position, startPosition);

            if (gameObject.transform.position.y < -3 || distanceFromPlatform > clearedDistance)
            {
                notClear = false;
            }
            else
            {
                notClear = true; // It's possible a block may fly straight up and land on the platform.
            }
        }

        /// <summary>
        /// Destroy the ball if it goes out of camera view.
        /// </summary>
        void OnBecameInvisible()
        {
            notClear = false;
        }
    }
}