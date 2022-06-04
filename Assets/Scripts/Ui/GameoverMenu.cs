using System;
using System.Collections;
using Controllers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CockroachHunter.Ui
{
    public class GameoverMenu: UiMenu
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private TMP_Text title;

        [Inject] private GameController _gameController;
        [Inject] private UiController _uiController;
        
        private void Start()
        {
            restartButton.transform.DOScaleX(0, 0);
            title.transform.DOScaleX(0, 0);
            
            restartButton.onClick.AddListener(() =>
            {
                _uiController.StartCoroutine(RestartGame());
            });
        }

        private IEnumerator RestartGame()
        {
            yield return _uiController.StartCoroutine(_uiController.OpenMenu<GameMenu>());
            _gameController.StartGame();
        }
        
        public override IEnumerator Open()
        {
            yield return base.Open();
            var sequence = DOTween.Sequence();
            sequence.Append(title.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutCirc));
            sequence.Join(restartButton.transform.DOScaleX(1, .5f).SetEase(Ease.OutCirc));
            yield return sequence.WaitForCompletion();
        }

        public override IEnumerator Close()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(title.transform.DOScaleX(0, 0.5f).SetEase(Ease.OutCirc));
            sequence.Join(restartButton.transform.DOScaleX(0, .5f).SetEase(Ease.OutCirc));
            yield return sequence.WaitForCompletion();
            yield return base.Close();
        }
    }
}