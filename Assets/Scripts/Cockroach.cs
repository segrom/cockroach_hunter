using System;
using CockroachHunter.ScriptableObjects;
using Controllers;
using UnityEngine;
using Zenject;

namespace CockroachHunter
{
    public class Cockroach: Movable
    {
        [SerializeField] private CockroachDescription cockroachDescription;
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
            SetSpeedsBySafety();
            Target.Value = _gameController.Finish.transform.position;
        }
        
        protected override void Update()
        {
            base.Update();

            var pointerPos = (Vector2)Camera.main.ScreenToWorldPoint( _input.Hunter.PointerPosition.ReadValue<Vector2>());

            if (_isInSave && Vector3.Distance(pointerPos, transform.position) < cockroachDescription.triggerRadius)
            {
                _isInSave = false;
                Target.Value = GetSaveTarget(pointerPos);
                SetSpeedsBySafety();
            }

            if (!_isInSave && Vector3.Distance(pointerPos, transform.position) > cockroachDescription.saveRadius)
            {
                _isInSave = true;
                Target.Value = _gameController.Finish.transform.position;
                SetSpeedsBySafety();
            }

            if (!_isInSave)
            {
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
            return transform.position + (transform.position - pointerPos).normalized * cockroachDescription.saveRadius;
        }

        private void SetSpeedsBySafety()
        {
            MovingSpeed =_isInSave? cockroachDescription.saveMovingSpeed : cockroachDescription.dangerMovingSpeed;
            RotationSpeed =_isInSave? cockroachDescription.saveRotationSpeed : cockroachDescription.dangerRotationSpeed;
        }
        
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

    }
}