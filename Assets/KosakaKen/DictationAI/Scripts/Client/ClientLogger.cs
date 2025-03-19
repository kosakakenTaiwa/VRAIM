using System.Linq;
using KosakaKen.DictationAI.Scripts.Client.Interfaces;
using KosakaKen.PythonPipe.Scripts.RemdisLog;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Client
{
    public class ClientLogger : MonoBehaviour
    {
        [SerializeField] private bool _isLogTimestamp;
        [SerializeField] private bool _isLogId;
        [SerializeField] private bool _isLogProducer;
        [SerializeField] private bool _isLogUpdateType;
        [SerializeField] private bool _isLogBody;
        
        private IClient _client;
        
        private void Start()
        {
            var clients = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IClient>();
            _client = clients.FirstOrDefault();
            
            _client?.Received
                .Subscribe(dto =>
                {
                    var option = new RemdisLogOption(
                        isLogTimeStamp: _isLogTimestamp,
                        isLogId: _isLogId,
                        isLogProducer: _isLogProducer,
                        isLogUpdateType: _isLogUpdateType,
                        isLogBody: _isLogBody
                        );
                    RemdisLogger.Log(dto, option);
                }).AddTo(this);
        }
    }
}
