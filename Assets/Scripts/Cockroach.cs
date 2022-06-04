using System;
using CockroachHunter.ScriptableObjects;
using Controllers;
using UnityEngine;
using Zenject;

namespace CockroachHunter
{
    public class Cockroach: Movable
    {
        public Vector2 RightUpperScreenCorner { get; set; }
        public Vector2 LeftDownScreenCorner { get; set; }
        
        [SerializeField] private CockroachDescription description;
        [Space]
        [SerializeField] private SpriteRenderer leftLegRenderer;
        [SerializeField] private SpriteRenderer rightLegRenderer;
        [SerializeField] private Sprite upLegsSprite, downLegsSprite;
        [SerializeField] private float legsSwitchDistance;
        [Space]
        [SerializeField] private Eye leftEye;
        [SerializeField] private Eye rightEye;
        
        [Inject] private GameController _gameController;

        private bool _isInSave = true;
        private bool _isLegsSwitched;
        private float _walkedDistance;
        private MainActions _input;

        protected override void Awake()
        {
            base.Awake();
            _input = new MainActions();
            SetSpeedsBySafety(Vector2.zero);
            Target.Value = _gameController.Finish.transform.position;
        }
        
        protected override void Update()
        {
            base.Update();

            var pointerPos = (Vector2)Camera.main.ScreenToWorldPoint( _input.Hunter.PointerPosition.ReadValue<Vector2>());

            if (_isInSave && Vector2.Distance(pointerPos, transform.position) < description.triggerRadius)
            {
                _isInSave = false;
                Target.Value = GetSaveTarget(pointerPos);
                SetSpeedsBySafety(pointerPos);
            }

            if (!_isInSave && Vector2.Distance(pointerPos, transform.position) > description.saveRadius)
            {
                _isInSave = true;
                Target.Value = _gameController.Finish.transform.position;
                SetSpeedsBySafety(pointerPos);
            }

            if (!_isInSave)
            {
                SetSpeedsBySafety(pointerPos);
                Target.Value = GetSaveTarget(pointerPos);
                leftEye.target = rightEye.target = pointerPos;
            }
            else
            {
                leftEye.target = rightEye.target = Target.Value;
            }
            
            _walkedDistance += Velocity.magnitude * Time.deltaTime;

            if (_walkedDistance > legsSwitchDistance)
            {
                _walkedDistance = 0;
                leftLegRenderer.sprite = _isLegsSwitched ? upLegsSprite : downLegsSprite;
                rightLegRenderer.sprite = _isLegsSwitched ? downLegsSprite : upLegsSprite;
                _isLegsSwitched = !_isLegsSwitched;
            }
        }

        private Vector2 GetSaveTarget(Vector3 pointerPos)
        {
            var pos = transform.position;
            var target = pos + (pos - pointerPos).normalized * description.saveRadius;
            
            if (target.x < LeftDownScreenCorner.x || target.x > RightUpperScreenCorner.x) 
                target = new Vector3(pos.x + (pos.x - target.x) * .5f, target.y);
            
            if (target.y < LeftDownScreenCorner.y || target.y > RightUpperScreenCorner.y) 
                target = new Vector3(target.x, pos.y + (pos.y - target.y) * .5f);
            return target;
        }

        private void SetSpeedsBySafety(Vector2 pointerPos)
        {
            if (_isInSave)
            {
                MovingSpeed = description.saveMovingSpeed;
                RotationSpeed = description.saveRotationSpeed;
                return;
            }
            var dangerRatio = Vector2.Distance(pointerPos, transform.position) / description.saveRadius;
            MovingSpeed = Mathf.Lerp( description.dangerMaxMovingSpeed, description.dangerMinMovingSpeed, dangerRatio );
            RotationSpeed = Mathf.Lerp(description.dangerMaxRotationSpeed, description.dangerMinRotationSpeed, dangerRatio);
        }
        
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(RightUpperScreenCorner, Vector3.one);
            Gizmos.DrawCube(LeftDownScreenCorner, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Target.Value, 1f);
        }
    }
}