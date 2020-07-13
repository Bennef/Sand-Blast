using UnityEngine;

namespace Scripts.Audio
{
    /// <summary>
    /// Handles the SFX in game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        private AudioSource aSrc;
        [SerializeField] private AudioClip _fire, _explosion, _tickTock, _fail, _success, _gameCompleted;

        public AudioClip Fire { get => _fire; set => _fire = value; }
        public AudioClip Explosion { get => _explosion; set => _explosion = value; }
        public AudioClip TickTock { get => _tickTock; set => _tickTock = value; }
        public AudioClip Fail { get => _fail; set => _fail = value; }
        public AudioClip Success { get => _success; set => _success = value; }
        public AudioClip GameCompleted { get => _gameCompleted; set => _gameCompleted = value; }

        void Start() => aSrc = GetComponent<AudioSource>();

        /// <summary>
        /// Assign the passed AudioClip and play it.
        /// </summary>
        public void PlaySound(AudioClip soundToPlay)
        {
            aSrc.PlayOneShot(soundToPlay);
        }
    }
}