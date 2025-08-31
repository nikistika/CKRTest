using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Core
{
    public interface IRequest
    {
        /// <summary>
        /// Выполняет асинхронный запрос.
        /// </summary>
        /// <param name="token">Токен для отмены операции.</param>
        UniTask ExecuteAsync(CancellationToken token);
        
        /// <summary>
        /// Отменяет запрос.
        /// </summary>
        void Cancel();
    }
}