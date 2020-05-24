using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandBlast
{
    

    public class Blocko : MonoBehaviour
    {
        public bool isActivel = true;

        void Update()
        {
            if (gameObject.transform.position.y < -3)
            {
                isActivel = false;
                //Debug.Log("deactivate");
                // Destroy after 3 seconds
            }
        }
    }
}