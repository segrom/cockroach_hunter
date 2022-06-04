using System;
using UnityEngine;

namespace CockroachHunter
{
    public class Eye : MonoBehaviour
    {
        /// <summary>
        /// Eye looking target
        /// </summary>
        public Vector2 target;

        [SerializeField] private Transform pupil;
        [SerializeField] private float radius;
        
        private void Update()
        {
            var direction = (target - (Vector2) transform.position).normalized;

            pupil.transform.position = (Vector2) transform.position + direction * radius;
        }
    }
}