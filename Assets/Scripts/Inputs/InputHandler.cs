using UnityEngine;

namespace Scripts.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        public bool GetMousePress()
        {
            if (Input.GetMouseButtonDown(0))
                return true;
            else
                return false;
        }        
    }
}