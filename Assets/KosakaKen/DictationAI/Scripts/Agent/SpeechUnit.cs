using System.Collections.Generic;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// エージェントの発話ユニット
    /// </summary>
    public class SpeechUnit
    {
        /// <summary>
        /// 発話する音声
        /// </summary>
        public readonly AudioClip AudioClip;
        /// <summary>
        /// 発話する音声に含まれていたIdのリスト。
        /// 複数の音声データをまとめて変換する場合を考慮してリストにしている。
        /// </summary>
        public readonly List<string> IdList;
        
        /// <param name="clip">発話する音声</param>
        /// <param name="idList">発話する音声に含まれていたIdのリスト</param>
        public SpeechUnit(AudioClip clip, List<string> idList)
        {
            AudioClip = clip;
            IdList = idList;
        }
    }
}