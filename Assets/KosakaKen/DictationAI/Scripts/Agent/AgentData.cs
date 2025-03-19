using System;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// エージェントが持つパラメータを格納するコンポーネント。
    /// MVPパターンのModelに該当。
    /// </summary>
    public class AgentData : MonoBehaviour, IAgentData
    {
        public IObservable<float> OnChangeAgentHeight => _agentHeight;
        public float AgentHeight => _agentHeight.Value;
        [SerializeField] private ReactiveProperty<float> _agentHeight = new (1f);

        public void ChangeHeight(float height)
        {
            _agentHeight.Value = height;
        }
    }
}