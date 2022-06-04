using System;
using Controllers;
using UnityEngine;
using Zenject;

namespace CockroachHunter
{
    public class FinishZone : MonoBehaviour
    {
        [SerializeField] private float radius;

        [Inject] private GameController _controller;
        
        private void FixedUpdate()
        {
            for (int i = 0; i < _controller.Cockroaches.Count; i++)
            {
                var cockroach = _controller.Cockroaches[i];
                if (!(Vector3.Distance(cockroach.transform.position, transform.position) < radius)) continue;
                _controller.Cockroaches.RemoveAt(i);
                Destroy(cockroach.gameObject);
            }

            if (_controller.Cockroaches.Count <= 0)
            {
                _controller.Gameover();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}