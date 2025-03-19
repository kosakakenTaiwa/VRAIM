using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KosakaKen.Utility.Scripts;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// エージェントの発話タイミングをスケジューリングするクラス
    /// </summary>
    [Serializable]
    public class AgentSpeechScheduler
    {
        [Serializable]
        public class Parameter
        {
            public float SpeechDelay => _speechDelay;
            [SerializeField, Range(-0.1f, 0.1f)]
            [Tooltip("エージェントが一つ一つの音声ファイルを読み上げるときの遅延 再生時にブツブツと音がする際は調整してください。")]
            private float _speechDelay = 0.001f;
        }
        [SerializeField] private Parameter _parameter;
        private readonly List<SpeechUnit> _speechUnitList = new();
        private AudioSource _audioSource;
        private CancellationToken _cancellationToken;
        private SpeechUnit _playingUnit = null;
        private bool _isSpeeching = false;
        
        public void Init(AudioSource audioSource, CancellationToken cancellationToken)
        {
            _audioSource = audioSource;
            _cancellationToken = cancellationToken;            
        }
        
        /// <summary>
        /// 音声の再生をスケジューリングする。
        /// </summary>
        /// <param name="unit"></param>
        public void Enqueue(SpeechUnit unit)
        {
            _speechUnitList.Add(unit);
            if (!_isSpeeching) Speech().Forget();
        }

        /// <summary>
        /// 音声を再生する。
        /// </summary>
        private async UniTask Speech()
        {
            if (_isSpeeching) return;
            Log("Speech Start");
            _isSpeeching = true;
            // 再生するものがなくなるまで再生
            while (_speechUnitList.Count > 0)
            {
                // 再生するものを取り出してリストから削除(Queue.Enqueue()と同じ動作)
                _playingUnit = _speechUnitList.FirstOrDefault();
                if(_playingUnit == null) break;
                _speechUnitList.Remove(_playingUnit);
                var audioClip = _playingUnit.AudioClip;
                _audioSource.PlayOneShot(audioClip);
                Log($"音声データを再生 : {_playingUnit.IdList.First()}");
                Log($"音声データの長さ : {audioClip.length}");
                if(audioClip == null) DebugLogger.LogEditorOnly("audioClip is null.", LogType.Warning);
                // TODO:Revokeされたときにキャンセルする処理を考える。
                // 音を再生し終わるまで待機
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(audioClip.length + _parameter.SpeechDelay), cancellationToken: _cancellationToken);
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    throw;
                }
                
            }
            Log("Speech End");
            _isSpeeching = false;
        }

        /// <summary>
        /// RemdisからRevokeされたときに実行する処理
        /// </summary>
        /// <param name="revokeUnitId"></param>
        public void Revoke(string revokeUnitId)
        {
            if (_isSpeeching)
            {
                _speechUnitList.Add(_playingUnit);
            }
            
            foreach (var searchTarget in _speechUnitList.ToList())
            {
                // 探索対象のIdリストからRevokeの対象を検索し、なければcontinueする
                if (searchTarget.IdList.All(id => id != revokeUnitId)) continue;
                
                // Revokeの対象を見つけた場合の処理
                if (searchTarget.AudioClip == _playingUnit.AudioClip)
                {
                    _audioSource.Stop();
                }
                else
                {
                    _speechUnitList.Remove(searchTarget);
                }
            }
        }

        private void Log(string message)
        {
            // Debug.Logの処理がビルド版のパフォーマンスに影響が出ないようにエディター上のみで動作するようにする
#if UNITY_EDITOR
            Debug.Log($"[AgentSpeech] {message}");
#endif
        }
    }
}