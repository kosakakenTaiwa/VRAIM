using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using KosakaKen.DictationAI.Scripts.Agent.StateMachine;
using KosakaKen.PythonPipe.Scripts;
using KosakaKen.PythonPipe.Scripts.Messages;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// エージェントとなるオブジェクトに追加されるコンポーネント
    /// </summary>
    public class Agent : MonoBehaviour, IAgent
    {
        public IObservable<RemdisMessageDto> ReceivedDialogueDialogue => _receivedDialogue;
        public IObservable<RemdisMessageDto> ReceivedTTS => _receivedTTS;
        public IObservable<RemdisEmotionMessageDto> ReceivedDialogue2 => _receivedDialogue2;
        public GameObject GameObject => gameObject;
        public string DialogueChannelName => _dialogueChannelName;
        public string TTSChannelName => _ttsChannelsName;
        public AgentStateController StateMachine => _agentStateController;
        
        [SerializeField] private string _dialogueChannelName = "dialogue";
        [SerializeField] private string _ttsChannelsName = "tts";
        [SerializeField] private string _dialogue2ChannelsName = "dialogue2";

        private readonly Subject<RemdisMessageDto> _receivedDialogue = new();
        private readonly Subject<RemdisMessageDto> _receivedTTS = new();
        private readonly Subject<RemdisEmotionMessageDto> _receivedDialogue2 = new();

        [Header("StateMachine")]
        [SerializeField] private AgentStateController _agentStateController;
        private CancellationToken _token;

        private void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
            RabbitMqClient.Instance?.SubscribeAsync(_dialogueChannelName, _receivedDialogue.OnNext, _token).Forget();
            RabbitMqClient.Instance?.SubscribeAsync(_ttsChannelsName, _receivedTTS.OnNext, _token).Forget();
            RabbitMqClient.Instance?.SubscribeAsync(_dialogue2ChannelsName, _receivedDialogue2.OnNext, _token).Forget();
            _agentStateController.Initialize(this);
        }
    }
}