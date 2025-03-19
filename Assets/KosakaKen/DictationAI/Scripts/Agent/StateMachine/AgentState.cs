using System;

namespace KosakaKen.DictationAI.Scripts.Agent.StateMachine
{
    /// <summary>
    /// エージェントのステート
    /// </summary>
    [Serializable]
    public enum AgentState
    {
        /// <summary>
        /// エージェントが話し続けているとき
        /// </summary>
        Speeching,
        /// <summary>
        /// エージェントが話を聞いているとき
        /// </summary>
        Listening,
    }
}