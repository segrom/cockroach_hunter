using System;
using System.Collections;
using System.Collections.Generic;
using CockroachHunter;
using CockroachHunter.ScriptableObjects;
using CockroachHunter.Ui;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// All roaches in current session
        /// </summary>
        public List<Cockroach> Cockroaches { get; private set; }
        /// <summary>
        /// Current session finish
        /// </summary>
        public FinishZone Finish => _currentFinish;

        [SerializeField] private Cockroach cockroachPrefab;
        [SerializeField] private FinishZone finishZonePrefab;
        [SerializeField] private GameObject startZonePrefab;
        
        [SerializeField] private GameDescription defaultGameDescription;
        
        [Inject] private DiContainer _diContainer;
        [Inject] private UiController _uiController;
        
        private FinishZone _currentFinish;
        private GameObject _currentStart;
        
        private void Start()
        {
            Cockroaches = new List<Cockroach>();
        }
        
        /// <summary>
        /// Remove all current session objects and spawn new from description
        /// </summary>
        /// <param name="description">New session description</param>
        public void StartGame(GameDescription description = null)
        {
            description ??= defaultGameDescription;

            var cam = Camera.main;

            _currentStart = _diContainer.InstantiatePrefab(startZonePrefab);
            _currentStart.transform.position = description.startPosition;
            
            _currentFinish = _diContainer.InstantiatePrefabForComponent<FinishZone>(finishZonePrefab);
            _currentFinish.transform.position = description.finishPosition;

            for (int i = 0; i < description.cockroachNumber; i++)
            {
                var newRoach = _diContainer.InstantiatePrefabForComponent<Cockroach>(cockroachPrefab);
                newRoach.transform.position = _currentStart.transform.position;
                newRoach.transform.rotation = Quaternion.Euler(0,0, Random.Range(0, 365f));
                newRoach.RightUpperScreenCorner = GetRightUpperScreenCorner(cam);
                newRoach.LeftDownScreenCorner = GetLeftDownScreenCorner(cam);
                Cockroaches.Add(newRoach);
            }
            
        }

        private Vector2 GetLeftDownScreenCorner(Camera cam)
        {
            return cam.ScreenToWorldPoint(Vector3.zero);
        }

        private Vector2 GetRightUpperScreenCorner(Camera cam)
        {
            return cam.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        }

        /// <summary>
        /// Remove all current session objects and go to gameover screen
        /// </summary>
        public void Gameover()
        {
            if (Cockroaches.Count > 0)
            {
                for (int i = 0; i < Cockroaches.Count; i++)
                {
                    Destroy(Cockroaches[i].gameObject);
                }
            }

            Cockroaches = new List<Cockroach>();
            
            if(_currentFinish) Destroy(_currentFinish.gameObject);
            if(_currentStart) Destroy(_currentStart.gameObject);

            _uiController.StartCoroutine(_uiController.OpenMenu<GameoverMenu>());
        }
    }
}