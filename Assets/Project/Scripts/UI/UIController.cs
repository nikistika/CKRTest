using System.Collections;
using Project.Scripts.Services;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    /// <summary>
    /// Контроллер для обновления интерфейса в зависимости от данных о погоде и фактах.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Inject] private DiContainer _container;  // Контейнер для инжекции зависимостей
        [Inject] private WeatherService _weatherService;
        [Inject] private FactsService _factsService;

        [SerializeField] private TextMeshProUGUI weatherText;
        [SerializeField] private Image weatherIcon;
        [SerializeField] private Transform factsContainer;
        [SerializeField] private FactItem factPrefab;

        void Start()
        {
            weatherText.text = "Загрузка...";
            
            _weatherService.Weather
                .Where(weather => weather != null) // Убедитесь, что weather не null
                .Subscribe(weather =>
                {
                    weatherText.text = $"{weather.Temperature}°F";
                    LoadWeatherIcon(weather.IconUrl);
                })
                .AddTo(this);
            
            _factsService.Facts.ObserveAdd()
                .Subscribe(fact => 
                {
                    var factItem = _container.InstantiatePrefabForComponent<FactItem>(factPrefab, factsContainer);
                    factItem.Initialize(fact.Value.Name, fact.Value.Description, fact.Value.Id);
                })
                .AddTo(this);
        }
        
        private void LoadWeatherIcon(string iconUrl)
        {
            StartCoroutine(DownloadIcon(iconUrl));
        }

        private IEnumerator DownloadIcon(string url)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    weatherIcon.sprite = sprite;
                }
                else
                {
                    Debug.LogError($"Failed to load weather icon: {www.error}");
                }
            }
        }
    }
}