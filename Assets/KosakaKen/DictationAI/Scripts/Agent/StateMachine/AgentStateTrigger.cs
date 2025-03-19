namespace KosakaKen.DictationAI.Scripts.Agent.StateMachine
{
    /// <summary>
    /// エージェントのステートを変更するトリガー
    /// </summary>
    public enum AgentStateTrigger
    {
        ToStartSpeech,
        ToEndSpeech,
        ToSpeeching,
        ToStartListen,
        ToEndListen,
        ToListening
    }
}