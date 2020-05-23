using UnityEngine;

namespace SandBlast
{ 
    /// <summary>
    /// Handles the firing of cannonballs.
    /// </summary>
    public class FireCannonBall : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody cannonballInstance;

        [SerializeField]
        [Range(10f, 80f)]
        private float angle = 45f;

        private AudioSource aSrc;

        void Start()
        {
            aSrc = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Handle input.
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    FireAtPoint(hitInfo.point);
                }
            }
        }

        /// <summary>
        /// Fire at point of click.
        /// </summary>
        /// <param name="point"></param>
        private void FireAtPoint(Vector3 point)
        {
            aSrc.Play();
            var velocity = BallisticVelocity(point, angle);

            cannonballInstance.transform.position = transform.position;
            cannonballInstance.isKinematic = false;
            cannonballInstance.velocity = velocity;
        }

        /// <summary>
        /// Calculate the required velocity for the cannonball.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vector3 BallisticVelocity(Vector3 destination, float angle)
        {
            Vector3 dir = destination - transform.position; // get Target Direction
            float height = dir.y; // get height difference
            dir.y = 0; // retain only the horizontal difference
            float dist = dir.magnitude; // get horizontal direction
            float a = angle * Mathf.Deg2Rad; // Convert angle to radians
            dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
            dist += height / Mathf.Tan(a); // Correction for small height differences

            // Calculate the velocity magnitude
            float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * dir.normalized; // Return a normalized vector.
        }
    }
}