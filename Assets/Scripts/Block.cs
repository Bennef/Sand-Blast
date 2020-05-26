using UnityEngine;

namespace SandBlast
{
    public class Block : MonoBehaviour
    {
        public bool notClear = true;

        /// <summary>
        /// If the blocks fall below the platform level, they are cleared.
        /// </summary>
        void Update()
        {
            if (gameObject.transform.position.y < -3)
            {
                notClear = false;
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