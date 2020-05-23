using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandBlast
{
    public class CannonBall : MonoBehaviour
    {
        /// <summary>
        /// Destroy the ball if it goes out of camera view.
        /// </summary>
        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
