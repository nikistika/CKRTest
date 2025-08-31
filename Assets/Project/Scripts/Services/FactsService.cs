using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Scripts.Services
{
    public class FactsService : IRequest
    {
        private const string FactsApiUrl = "https://dogapi.dog/api/v2/breeds";
        private readonly ReactiveCollection<Fact> _facts = new ReactiveCollection<Fact>();

        public IReadOnlyReactiveCollection<Fact> Facts => _facts;

        /// <summary>
        /// Выполняет асинхронный запрос для получения списка фактов.
        /// </summary>
        /// <param name="token">Токен для отмены операции.</param>
        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(FactsApiUrl);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                var facts = FactResponse.FromJson(json);

                if (facts != null)
                {
                    _facts.Clear();
                    foreach (var fact in facts.Take(10))
                    {
                        _facts.Add(fact);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to fetch facts: " + request.error);
            }
        }

        public void Cancel()
        {
        }
    }

    [System.Serializable]
public class FactResponse
{
    public List<BreedData> Data;
    public Links Links;
    
    public static List<Fact> FromJson(string json)
    {
        var factResponse = JsonConvert.DeserializeObject<FactResponse>(json);
        
        if (factResponse != null && factResponse.Data != null && factResponse.Data.Count > 0)
        {
            var facts = new List<Fact>();
            
            foreach (var breed in factResponse.Data)
            {
                var fact = new Fact
                {
                    Id = breed.Id,
                    Name = breed.Attributes.Name,
                    Description = breed.Attributes.Description,
                };
                
                facts.Add(fact);
            }

            return facts;
        }

        return null;
    }
}

[System.Serializable]
public class BreedData
{
    public string Id;
    public string Type;
    public BreedAttributes Attributes;
}

[System.Serializable]
public class BreedAttributes
{
    public string Name;
    public string Description;
    public LifeSpan Life;
    public WeightRange MaleWeight;
    public WeightRange FemaleWeight;
    public bool Hypoallergenic;
}

[System.Serializable]
public class LifeSpan
{
    public int Min;
    public int Max;
}

[System.Serializable]
public class WeightRange
{
    public int Min;
    public int Max;
}

[System.Serializable]
public class Links
{
    public string Self;
    public string Current;
    public string Next;
    public string Last;
}

[System.Serializable]
public class Fact
{
    public string Id;
    public string Name;
    public string Description;
}

}