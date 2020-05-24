using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Handles the firing of cannonballs.
    /// Credit to Jason Weimann for help with the basics of this script.
    /// </summary>
    public class Cannon : MonoBehaviour
    {
        [SerializeField]
        [Range(10f, 80f)]
        private float angle = 25f;

        private AudioSource aSrc;

        [SerializeField]
        private Transform cannonball;
        private Rigidbody rb;

        private GameManager gameManager;


        void Start()
        {
            aSrc = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Fire at point of click.
        /// </summary>
        /// <param name="point"></param>
        public void Fire()
        {
            aSrc.Play();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var velocity = BallisticVelocity(hitInfo.point, angle);

                var ball = Instantiate(cannonball, transform.position, Quaternion.identity);
                rb = cannonball.GetComponent<Rigidbody>();
                ball.GetComponent<Rigidbody>().velocity = velocity;
            }
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