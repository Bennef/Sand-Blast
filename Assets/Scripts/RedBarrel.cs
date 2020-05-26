using System.Collections;
using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Red barrels will explode when hit by the ball, causing a wave.
    /// </summary>
    public class RedBarrel : MonoBehaviour
    {
        public GameObject explosion;

        public AudioSource aSrc;

        private MeshCollider meshColl;
        private MeshRenderer meshRen;

        /// <summary>
        /// When the cannonball hits the red barrel, make it explode.
        /// </summary>
        /// <param name="other">The other thing colliding with this thing.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "CannonBall")
            {
                aSrc.Play();
                Instantiate(explosion, transform.position, transform.rotation);
                StartCoroutine(Explode());
            }
        }

        /// <summary>
        /// Disable Mesh and collider before destroying to allow sound to play.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Explode()
        {
            meshColl = GetComponent<MeshCollider>();
            meshColl.enabled = false;
            meshRen = GetComponent<MeshRenderer>();
            meshRen.enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }
    }
}