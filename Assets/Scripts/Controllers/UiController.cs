using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CockroachHunter.Ui;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class UiController: MonoBehaviour
    {
        [SerializeField] private GameObject mainUiPrefab;

        [Inject] private DiContainer _diContainer;

        private readonly Type _startMenuType = typeof(MainMenu);
        private List<UiMenu> _menus;
        private UiMenu _currentMenu;
        private GameObject _uiRoot;

        private void Start()
        {
            _uiRoot = _diContainer.InstantiatePrefab(mainUiPrefab);
            _uiRoot.transform.parent = transform;
            for (int i = 0; i < _uiRoot.transform.childCount; i++)
            {
                _uiRoot.transform.GetChild(i).gameObject.SetActive(true);
            }

            _menus = new List<UiMenu>();
            _menus = _uiRoot.GetComponentsInChildren<UiMenu>().ToList();
            print($"menus founded {_menus.Count}");
            
            foreach (UiMenu menu in _menus)
            {
                if(menu.GetType() == _startMenuType)
                {
                    _currentMenu = menu;
                }
                menu.gameObject.SetActive(false);
            }

            StartCoroutine(_currentMenu.Open());
        }

        public IEnumerator OpenMenu<T>()
        where T: UiMenu
        {
            foreach (UiMenu menu in _menus)
            {
                if (menu is not T) continue;
                yield return StartCoroutine(_currentMenu.Close());
                yield return StartCoroutine(menu.Open());
                _currentMenu = menu;
                break;
            }
        }
    }
}