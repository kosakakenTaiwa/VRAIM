using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using KosakaKen.PythonPipe.Scripts.Messages;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    public class AgentAnimationPlayer : MonoBehaviour
    {
        [Serializable]
        private sealed class AgentAnimationProperty
        {
            public ActionType ActionType;
            public string TriggerName;
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private List<AgentAnimationProperty> _agentAnimationProperties = new();
        private IAgent _agent;
        private bool _isPlaying = false;
        
        private void Start()
        {
            _agent = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IAgent>().FirstOrDefault();
            _agent?.ReceivedDialogue2
                .Subscribe(message => PlayAnimation(message.ActionType))
                .AddTo(this);
        }

        private void PlayAnimation(ActionType actionType)
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
                return;
            }
            
            foreach (var animationProperty in _agentAnimationProperties)
            {
                if (animationProperty.ActionType != actionType) continue;
                
                // これ以降はactionTypeとanimationProperty.ActionTypeが一致している前提で処理を進める
                _animator?.SetTrigger(animationProperty.TriggerName);       
                break;
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void PlayAnimationPublic(ActionType actionType)
        {
            PlayAnimation(actionType);
        }
    }
}