using UnityEngine;

namespace SandBlast
{
    public class Block : MonoBehaviour
    {
        public bool isActive = true;

        void Update()
        {
            if (gameObject.transform.position.y < -3)
            {
                isActive = false;
            }
        }
    }
}