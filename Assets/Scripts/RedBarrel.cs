using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Red barrels will explode when hit by the ball, causing a wave.
    /// </summary>
    public class RedBarrel : MonoBehaviour
    {
        public GameObject brokenBarrel;

        public float radius = 5.0F;
        public float power = 100.0F;

        private Rigidbody rb;

        /// <summary>
        /// When the cannonball hits the red barrel, make it explode.
        /// </summary>
        /// <param name="other">The other thing colliding with this thing.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "CannonBall")
            {
                rb = GetComponent<Rigidbody>();
                rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                Instantiate(brokenBarrel, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}