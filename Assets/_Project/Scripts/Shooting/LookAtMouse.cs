using Scripts.Core;
using UnityEngine;

namespace Scripts.Shooting
{
    /// <summary>
    /// Make the cannon look at the mouse.
    /// </summary>
    public class LookAtMouse : MonoBehaviour
    {
        GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        /// <summary>
        /// Point the cannon in the direction of the mouse cursor.
        /// </summary>
        void Update()
        {
            if (!_gameManager.GameOver)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    transform.LookAt(hitInfo.point); // TO DO - clamp this angle so the cannon only points within a certain range.
                }
            }
        }
    }
}
