using System;
using CockroachHunter.Utils;
using UnityEngine;

namespace CockroachHunter
{
    public class Movable : MonoBehaviour
    {
        protected BindingValue<Vector2> Target { get; set; }
        protected Vector2 Velocity { get; private set; }
        protected float MovingSpeed { get; set; }
        protected float RotationSpeed { get; set; }
        
        
        private Vector2 _lastPosition;
        private Quaternion _targetRotation;

        protected virtual void Awake()
        {
            Target = new BindingValue<Vector2>();
            _targetRotation = transform.rotation;
        }

        protected virtual void Update()
        {
            Move();
            
            Velocity = ((Vector2)transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;
        }

        private void Move()
        {
            if (Vector2.Distance(transform.position, Target.Value) > 0.01f)
            {
                transform.position += transform.up * MovingSpeed * Time.deltaTime;
                var rotationDirection =
                    Vector2.Dot(-transform.right, Target.Value - (Vector2)transform.position);
                rotationDirection /= Mathf.Abs(rotationDirection);
                transform.rotation *= Quaternion.Euler(0,0,  rotationDirection* RotationSpeed * Time.deltaTime);
            }
        }
    }
}