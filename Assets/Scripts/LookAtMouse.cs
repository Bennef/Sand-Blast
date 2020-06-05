using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Make the cannon look at the mouse.
    /// </summary>
    public class LookAtMouse : MonoBehaviour
    {
        /// <summary>
        /// Point the cannon in the direction of the mouse cursor.
        /// </summary>
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                transform.LookAt(hitInfo.point); // TO DO - clamp this angle so the cannon only points within a certain range.
            }
        }
    }
}
