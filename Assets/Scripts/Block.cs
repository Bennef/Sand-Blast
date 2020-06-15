﻿using UnityEngine;

namespace SandBlast
{
    public class Block : MonoBehaviour
    {
        public bool cleared = false;

        private readonly float clearedDistance = 15f;

        private Vector3 startPosition;
        

        /// <summary>
        /// Store the block starting position. We will need this to determine if the block has been sufficiently displaced to clear it.
        /// </summary>
        void Start()
        {
            startPosition = gameObject.transform.position;
        }

        /// <summary>
        /// If the blocks fall below the platform level or are a given distance away, they are cleared.
        /// </summary>
        void Update()
        {
            float distanceFromStartPosition = Vector3.Distance(gameObject.transform.position, startPosition);
            cleared = gameObject.transform.position.y < -3 || distanceFromStartPosition > clearedDistance ? true : false;
        }

        /// <summary>
        /// Destroy the block if it goes out of camera view.
        /// </summary>
        void OnBecameInvisible()
        {
            cleared = false;
        }
    }
}