using System;

namespace KosakaKen.DictationAI.Scripts.Agent.Interfaces
{
    public interface IAgentData
    {
        /// <summary>
        /// エージェントの身長を変更したときに発火されるイベント
        /// </summary>
        public IObservable<float> OnChangeAgentHeight { get; }
        /// <summary>
        /// エージェントの身長
        /// </summary>
        public float AgentHeight { get; }
        /// <summary>
        /// エージェントの身長を変更する
        /// </summary>
        /// <param name="height">変更後のエージェントの身長</param>
        public void ChangeHeight(float height);
    }
}