using Project.Scripts.Core;
using Project.Scripts.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class FactItem : MonoBehaviour
    {
        [Inject] private RequestQueue _requestQueue;
        [Inject] private FactsService _factsService;
        [Inject] private PopupManager _popupManager;

        private string _factId;
        private string _description;

        [SerializeField] private TextMeshProUGUI factText;

        /// <summary>
        /// Инициализирует элемент с текстом и идентификатором факта.
        /// </summary>
        /// <param name="fact">Текст факта.</param>
        /// <param name="factId">Идентификатор факта для подробной информации.</param>
        public void Initialize(string fact, string description, string factId)
        {
            factText.text = fact;
            _factId = factId;
            _description = description;
        }

        public void OnClick()
        {
            _requestQueue.CancelAll();
            _requestQueue.Enqueue(new FactDetailRequest(new FactDetail(factText.text, _description), _popupManager));
        }
    }
}