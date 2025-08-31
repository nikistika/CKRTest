using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class RequestQueue
    {
        private readonly Queue<IRequest> _queue = new Queue<IRequest>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isProcessing;

        public void Enqueue(IRequest request)
        {
            _queue.Enqueue(request);
            ProcessQueue();
        }

        public void CancelAll()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _queue.Clear();
            _isProcessing = false;
        }

        private async void ProcessQueue()
        {
            if (_isProcessing || _queue.Count == 0) return;

            _isProcessing = true;

            while (_queue.Count > 0)
            {
                var request = _queue.Dequeue();
                try
                {
                    await request.ExecuteAsync(_cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Request was canceled");
                }
            }

            _isProcessing = false;
        }
    }
}