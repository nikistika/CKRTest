using Project.Scripts.Core;
using Project.Scripts.Services;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private FactItem factPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<WeatherService>().AsSingle();
            Container.Bind<FactsService>().AsSingle();
            Container.Bind<RequestQueue>().AsSingle();
            Container.Bind<PopupManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TabController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UIController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TabAnimations>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FactItem>().FromComponentInNewPrefab(factPrefab).AsTransient();
        }
    }
}