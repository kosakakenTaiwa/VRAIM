using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KosakaKen.DictationAI.Scripts.Client.Interfaces;
using KosakaKen.PythonPipe.Scripts;
using KosakaKen.PythonPipe.Scripts.Messages;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Client
{
    public class Client : MonoBehaviour, IClient
    {
        public IObservable<RemdisMessageDto> Received => _received;
        public GameObject GameObject => gameObject;
        public string ChannelName => _channelName;
        
        [SerializeField] private string _channelName = "asr";
        private Subject<RemdisMessageDto> _received = new();
        private CancellationToken _token;

        private void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
            RabbitMqClient.Instance.SubscribeAsync(_channelName, _received.OnNext, _token).Forget();
        }
    }
}