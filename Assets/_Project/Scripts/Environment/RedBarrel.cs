using Scripts.Audio;
using System.Collections;
using UnityEngine;

namespace Scripts.Environment
{
    /// <summary>
    /// Red barrels will explode when hit by the ball, causing a wave.
    /// </summary>
    public class RedBarrel : MonoBehaviour
    {
        [SerializeField] private GameObject _explosion;
        [SerializeField] private MeshCollider _meshColl;
        [SerializeField] private MeshRenderer _meshRen;
        private SFXManager _sFXManager;

        public GameObject Explosion { get => _explosion; set => _explosion = value; }

        void Start()
        {
            _meshColl = GetComponent<MeshCollider>();
            _meshRen = GetComponent<MeshRenderer>();
            _sFXManager = FindObjectOfType<SFXManager>();
        }

        /// <summary>
        /// When the cannonball hits the red barrel, make it explode.
        /// </summary>
        /// <param name="other">The other thing colliding with this thing.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("CannonBall"))
                StartCoroutine(Explode());
        }

        /// <summary>
        /// Disable Mesh and collider before destroying to allow sound to play.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Explode()
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            _sFXManager.PlaySound(_sFXManager.Explosion);
            _meshColl.enabled = false;
            _meshRen.enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }
    }
}