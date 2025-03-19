using System;
using KosakaKen.DictationAI.Scripts.Agent.StateMachine;
using KosakaKen.PythonPipe.Scripts;
using KosakaKen.PythonPipe.Scripts.Messages;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent.Interfaces
{
    public interface IAgent
    {
        /// <summary>
        /// dialogueからデータを受け取ったときに発火されるイベント
        /// </summary>
        public IObservable<RemdisMessageDto> ReceivedDialogueDialogue { get; }
        
        /// <summary>
        /// ttsからデータを受け取ったときに発火されるイベント
        /// </summary>
        public IObservable<RemdisMessageDto> ReceivedTTS { get; }
        
        /// <summary>
        /// dialogue2からデータを受け取ったときに発火されるイベント
        /// </summary>
        public IObservable<RemdisEmotionMessageDto> ReceivedDialogue2 { get; }
        
        /// <summary>
        /// AgentのGameObject
        /// </summary>
        public GameObject GameObject { get; }
        
        /// <summary>
        /// dialogueのチャンネル名
        /// </summary>
        public string DialogueChannelName { get; }
        
        /// <summary>
        /// ttsのチャンネル名
        /// </summary>
        public string TTSChannelName { get; }
        
        /// <summary>
        /// エージェントのステートマシン
        /// </summary>
        public AgentStateController StateMachine { get; }
    }
}