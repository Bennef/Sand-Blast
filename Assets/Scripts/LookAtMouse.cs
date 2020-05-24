using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Make the cannon look at the mouse.
    /// </summary>
    public class LookAtMouse : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                transform.LookAt(hitInfo.point); // TO DO - clamp this angle.
            }
        }
    }
}
