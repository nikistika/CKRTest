using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Scripts.Services
{
    public class WeatherService : IRequest
    {
        private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private WeatherData _cachedWeather;
        private readonly ReactiveProperty<WeatherData> _weather = new ReactiveProperty<WeatherData>();

        public IReadOnlyReactiveProperty<WeatherData> Weather => _weather;

        /// <summary>
        /// Выполняет асинхронный запрос на получение данных о погоде.
        /// </summary>
        /// <param name="token">Токен для отмены операции.</param>
        public async UniTask ExecuteAsync(CancellationToken token)
        {
            if (_cachedWeather != null)
            {
                _weather.Value = _cachedWeather;
                return;
            }

            using var request = UnityWebRequest.Get(WeatherApiUrl);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                var data = WeatherData.FromJson(json);
                _cachedWeather = data;
                _weather.Value = data;
            }
            else
            {
                Debug.LogError("Failed to fetch weather data: " + request.error);
            }
        }

        public void ClearCache()
        {
            _cachedWeather = null;
        }

        public void Cancel()
        {
        }
    }
    
    [System.Serializable]
    public class WeatherData
    {
        public string Name;
        public float Temperature;
        public string TemperatureUnit;
        public string IconUrl;
        public string ShortForecast;
        public string DetailedForecast;
        
        public static WeatherData FromJson(string json)
        {
            var weatherResponse = JsonUtility.FromJson<WeatherApiResponse>(json);
            if (weatherResponse != null && weatherResponse.properties.periods.Length > 0)
            {
                var period = weatherResponse.properties.periods[0];
                return new WeatherData
                {
                    Name = period.name,
                    Temperature = period.temperature,
                    TemperatureUnit = period.temperatureUnit,
                    IconUrl = period.icon,
                    ShortForecast = period.shortForecast,
                    DetailedForecast = period.detailedForecast
                };
            }
            return null;
        }
    }
    
    [System.Serializable]
    public class WeatherApiResponse
    {
        public WeatherProperties properties;
    }

    [System.Serializable]
    public class WeatherProperties
    {
        public WeatherPeriod[] periods;
    }

    [System.Serializable]
    public class WeatherPeriod
    {
        public string name;
        public float temperature;
        public string temperatureUnit;
        public string icon;
        public string shortForecast;
        public string detailedForecast;
    }
}