using Controllers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private GameController gameControllerPrefab;
        [SerializeField] private UiController uiControllerPrefab;
        private GameController _gameController;
        private UiController _uiController;
        
        public override void InstallBindings()
        {
            _uiController = Container.InstantiatePrefabForComponent<UiController>(uiControllerPrefab.gameObject);
            Container.Bind<UiController>().FromInstance(_uiController).AsSingle().NonLazy();
            
            _gameController = Container.InstantiatePrefabForComponent<GameController>(gameControllerPrefab.gameObject);
            Container.Bind<GameController>().FromInstance(_gameController).AsSingle().NonLazy();
        }
    }
}