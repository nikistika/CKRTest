using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.UI;

namespace Project.Scripts.Core
{
    public class FactDetailRequest : IRequest
    {
        private readonly FactDetail _detail;
        private readonly PopupManager _popupManager;

        public FactDetailRequest(FactDetail detail , PopupManager popupManager)
        {
            _popupManager = popupManager;
            _detail = detail;
        }

        /// <summary>
        /// Выполняет запрос для получения данных о факте.
        /// </summary>
        /// <param name="token">Токен для отмены операции.</param>
        public async UniTask ExecuteAsync(CancellationToken token)
        {
            await UniTask.Delay(500, cancellationToken: token);
            _popupManager.ShowPopup(_detail);
        }

        public void Cancel()
        {
        }
    }

    [System.Serializable]
    public class FactDetail
    {
        public string Title;
        public string Description;

        public FactDetail(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}