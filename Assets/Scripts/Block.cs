using UnityEngine;

namespace SandBlast
{
    public class Block : MonoBehaviour
    {
        public bool isActive = true;

        /// <summary>
        /// If the blocks fall below the platform level, they are cleared.
        /// </summary>
        void Update()
        {
            if (gameObject.transform.position.y < -3)
            {
                isActive = false;
            }
        }

        /// <summary>
        /// Destroy the ball if it goes out of camera view.
        /// </summary>
        void OnBecameInvisible()
        {
            isActive = false;
            Destroy(gameObject);
        }
    }
}