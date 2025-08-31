using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using Project.Scripts.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private Button weatherTabButton;
        [SerializeField] private Button factsTabButton;
        
        [Inject] private RequestQueue _requestQueue;
        [Inject] private WeatherService _weatherService;
        [Inject] private FactsService _factsService;
        [Inject] private PopupManager _popupManager;
        [Inject] private TabAnimations _tabAnimations;

        private ReactiveProperty<int> _activeTab = new ReactiveProperty<int>(0);
        private CancellationTokenSource _weatherRequestCancellationTokenSource;

        /// <summary>
        /// Переключает вкладку на указанную.
        /// </summary>
        /// <param name="index">Индекс вкладки (0 — погода, 1 — факты).</param>
        public void SwitchTab(int index)
        {
            _activeTab.Value = index;
        }

        void Start()
        {
            _activeTab
                .Subscribe(tab =>
                {
                    _requestQueue.CancelAll();

                    if (tab == 0)
                    {
                        _weatherService.ClearCache();
                        _requestQueue.Enqueue(_weatherService);
                        _tabAnimations.SwitchToWeatherTab();
                        StartWeatherRequests(); 
                    }
                    else if (tab == 1)
                    {
                        _requestQueue.Enqueue(_factsService);
                        _tabAnimations.SwitchToFactsTab();
                        CancelWeatherRequests();
                    }
                })
                .AddTo(this);
            
            weatherTabButton.onClick.AddListener(() => SwitchTab(0));
            factsTabButton.onClick.AddListener(() => SwitchTab(1));
        }
        
        private async void StartWeatherRequests()
        {
            if (_weatherRequestCancellationTokenSource != null)
            {
                _weatherRequestCancellationTokenSource.Cancel();
            }

            _weatherRequestCancellationTokenSource = new CancellationTokenSource();
            var token = _weatherRequestCancellationTokenSource.Token;
            
            while (!token.IsCancellationRequested)
            {
                _requestQueue.Enqueue(_weatherService);
                try
                {
                    await UniTask.Delay(5000, cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Weather request was cancelled.");
                    return;
                }
            }
        }

        // Остановить регулярные запросы погоды
        private void CancelWeatherRequests()
        {
            _weatherRequestCancellationTokenSource?.Cancel();
        }
    }

}