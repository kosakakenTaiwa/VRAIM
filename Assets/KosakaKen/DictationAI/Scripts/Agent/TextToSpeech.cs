using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using KosakaKen.PythonPipe.Scripts;
using KosakaKen.PythonPipe.Scripts.Audio;
using KosakaKen.PythonPipe.Scripts.Messages;
using KosakaKen.Utility.Scripts;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// RemdisのTTS部から受け取ったデータを再生するコンポーネント
    /// </summary>
    public class TextToSpeech : MonoBehaviour
    {
        [SerializeField] private AudioSource _voiceAudioSource;
        [Tooltip("音声ファイルを貯めておく個数。ここで指定した個数分、音声ファイルが溜まったら一気に音声ファイルにして再生する。")]
        [SerializeField] private int _speechBlockCount = 5;
        [SerializeField] private AgentSpeechScheduler _speechScheduler;
        private Dictionary<string, byte[]> _ttsDictionary = new();
        private IAgent _agent;
        private CancellationToken _token;
        
        private void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
            _speechScheduler.Init(_voiceAudioSource, _token);
            _agent = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IAgent>().FirstOrDefault();
            _agent?.ReceivedTTS
                .Subscribe(ReceivedTTS)
                .AddTo(this);
        }

        private void ReceivedTTS(RemdisMessageDto remdisMessage)
        {
            // これ以降の処理は音声データのやりとりであることを前提にする。
            if (remdisMessage.data_type != RemdisDataType.AUDIO) return;
            
            switch (remdisMessage.update_type)
            {
                case RemdisUpdateType.ADD:
                    // 空のメッセージを音声変換する意味がないので早期リターンする
                    if (remdisMessage.body == "") return;
                    // ある程度音声データが溜まったら再生する
                    if (_ttsDictionary.Count > _speechBlockCount)
                    {
                        PlayAgentVoice(_ttsDictionary);
                        _ttsDictionary.Clear();
                    }
                    _ttsDictionary.Add(remdisMessage.id, Convert.FromBase64String(remdisMessage.body));
                    return;
                case RemdisUpdateType.COMMIT:
                    if(_ttsDictionary.Count <= 0) return;
                    PlayAgentVoice(_ttsDictionary);
                    _ttsDictionary.Clear();
                    return;
                case RemdisUpdateType.REVOKE:
                    _speechScheduler.Revoke(remdisMessage.id);
                    return;
            }
        }

        /// <summary>
        /// エージェントの音声を再生する。
        /// </summary>
        /// <param name="ttsDictionary"></param>
        private void PlayAgentVoice(Dictionary<string, byte[]> ttsDictionary)
        {
            var idList = new List<string>();
            var audioData = new List<byte>();

            foreach (var ttsData in ttsDictionary)
            {
                idList.Add(ttsData.Key);
                audioData.AddRange(ttsData.Value);
            }

            var audioClip = AudioConvert.ToAudioClipFromByteArray(audioData.ToArray(), "AgentVoice");
            _speechScheduler.Enqueue(new SpeechUnit(audioClip, idList));
        }

        private void Log(string message)
        {
            DebugLogger.LogEditorOnly($"[TTS] {message}");
        }
    }
}