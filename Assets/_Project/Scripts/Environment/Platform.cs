using UnityEngine;

public class Platform : MonoBehaviour
{
    private AudioSource _aSrc;

    void Start() => _aSrc = GetComponent<AudioSource>();

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("CannonBall")) 
            _aSrc.Play();
    }
}