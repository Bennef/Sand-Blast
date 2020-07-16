using UnityEngine;

namespace Scripts.Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private bool _cleared;
        [SerializeField] private readonly float _clearedDistance = 15f;
        private Vector3 _startPosition;

        public bool Cleared => _cleared;

        /// <summary>
        /// Store the block starting position. We will need this to determine if the block has been sufficiently displaced to clear it.
        /// </summary>
        void Start() => _startPosition = gameObject.transform.position;
 
        /// <summary>
        /// If the blocks fall below the platform level or are a given distance away, they are cleared.
        /// </summary>
        void Update()
        {
            float distanceFromStartPosition = Vector3.Distance(gameObject.transform.position, _startPosition);
            _cleared = gameObject.transform.position.y < -3 || distanceFromStartPosition > _clearedDistance;
        }

        /// <summary>
        /// Destroy the block if it goes out of camera view.
        /// </summary>
        void OnBecameInvisible() => _cleared = false;
    }
}