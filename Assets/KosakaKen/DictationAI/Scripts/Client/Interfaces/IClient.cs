using System;
using KosakaKen.PythonPipe.Scripts;
using KosakaKen.PythonPipe.Scripts.Messages;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Client.Interfaces
{
    public interface IClient
    {
        /// <summary>
        /// Remdisからのメッセージを受け取ったときに発火されるイベント
        /// どのメッセージを受信しているかはChannelNameを参照
        /// </summary>
        public IObservable<RemdisMessageDto> Received { get; }
        /// <summary>
        /// クライアント自身のオブジェクト
        /// </summary>
        public GameObject GameObject { get; }
        /// <summary>
        /// 受信しているチャンネル名
        /// </summary>
        public string ChannelName { get; }
    }
}