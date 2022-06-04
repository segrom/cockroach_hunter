using System;
using System.Collections;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CockroachHunter.Ui
{
    public class MainMenu: UiMenu
    {
        [SerializeField] private Button startButton;
        
        [Inject] private GameController _gameController;
        [Inject] private UiController _uiController;

        private void Start()
        {
            startButton.transform.DOScaleX(0, 0f);
            startButton.onClick.AddListener(() =>
            {
                _uiController.StartCoroutine(StartGame());
            });
        }

        private IEnumerator StartGame()
        {
            yield return _uiController.StartCoroutine(_uiController.OpenMenu<GameMenu>());
            _gameController.StartGame();
        }
        
        public override IEnumerator Open()
        {
            yield return base.Open();
            yield return startButton.transform.DOScaleX( 1, .5f).SetEase(Ease.OutCirc).WaitForCompletion();
        }
        
        public override IEnumerator Close()
        {
            yield return startButton.transform.DOScaleX( 0, .5f).SetEase(Ease.OutCirc).WaitForCompletion();
            yield return base.Close();
        }
    }
}