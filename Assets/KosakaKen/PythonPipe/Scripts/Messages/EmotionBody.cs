using System;

namespace KosakaKen.PythonPipe.Scripts.Messages
{
    [Serializable]
    public class EmotionBody
    {
        public ExpressionType ExpressionType
        {
            get
            {
                return expression switch
                {
                    "normal" => ExpressionType.Normal,
                    "joy" => ExpressionType.Joy,
                    "impressed" => ExpressionType.Impressed,
                    "convinced" => ExpressionType.Convinced,
                    "thinking" => ExpressionType.Thinking,
                    "sleepy" => ExpressionType.Sleepy,
                    "suspicion" => ExpressionType.Suspicion,
                    "compassion" => ExpressionType.Compassion,
                    "embarrassing" => ExpressionType.Embarrassing,
                    "anger" => ExpressionType.Anger,
                    _ => ExpressionType.Normal
                };
            }
        }
        public ActionType ActionType
        {
            get
            {
                return action switch
                {
                    "wait" => ActionType.Wait,
                    "listening" => ActionType.Listening,
                    "nod" => ActionType.Nod,
                    "head_tilt" => ActionType.HeadTilt,
                    "thinking" => ActionType.Thinking,
                    "light_greeting" => ActionType.LightGreeting,
                    "greeting" => ActionType.Greeting,
                    "wavehand" => ActionType.WaveHand,
                    "wavehands" => ActionType.WaveHands,
                    "lookaround" => ActionType.LookAround,
                    _ => ActionType.Wait
                };
            }
        }

        public string expression;
        public string action;
    }
}