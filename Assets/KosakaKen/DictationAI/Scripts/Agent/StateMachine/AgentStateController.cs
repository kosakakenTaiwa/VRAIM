using System;
using KosakaKen.Utility.Scripts;

namespace KosakaKen.DictationAI.Scripts.Agent.StateMachine
{
    /// <summary>
    /// エージェントのステートマシン
    /// </summary>
    [Serializable]
    public class AgentStateController : StateMachineControllerBase<AgentState, AgentStateTrigger>, IAgentStateGetter
    {
        public AgentState CurrentState => _stateMachine.CurrentState;
        
        protected override void SetUpTransition()
        {
            _stateMachine.AddTransition(AgentState.Speeching, AgentState.Listening, AgentStateTrigger.ToListening);
            _stateMachine.AddTransition(AgentState.Listening, AgentState.Speeching, AgentStateTrigger.ToSpeeching);
        }
    }
}