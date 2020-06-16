﻿using UnityEngine;

namespace SandBlast
{
    /// <summary>
    /// Handles the firing of cannonballs.
    /// Credit to Jason Weimann for help with the basics of this script.
    /// </summary>
    public class Cannon : MonoBehaviour
    {
        [SerializeField]
        private float force = 40f;

        private AudioSource aSrc;

        [SerializeField]
        private Transform cannonball;
        private Rigidbody rb;
        

        void Start()
        {
            aSrc = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Fire at point of click.
        /// </summary>
        public void Fire()
        {
            aSrc.Play();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var velocity = BallisticVelocity(hitInfo.point, force);

                var ball = Instantiate(cannonball, transform.position, Quaternion.identity);
                rb = cannonball.GetComponent<Rigidbody>();
                ball.GetComponent<Rigidbody>().velocity = velocity;
            }
        }

        /// <summary>
        /// Calculate the required velocity for the cannonball.
        /// </summary>
        /// <param name="destination">Destination will be the hit point of the raycast.</param>
        /// <param name="force">Force of the cannonball.</param>
        /// <returns>Normalised vector of velocity for ball</returns>
        private Vector3 BallisticVelocity(Vector3 destination, float force)
        {
            Vector3 dir = destination - transform.position; // get Target Direction
            return dir.normalized * force; // Return a normalized vector.
        }
    }
}