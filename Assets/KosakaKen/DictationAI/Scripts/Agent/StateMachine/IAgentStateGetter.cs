namespace KosakaKen.DictationAI.Scripts.Agent.StateMachine
{
    public interface IAgentStateGetter
    {
        public AgentState CurrentState { get; }
    }
}