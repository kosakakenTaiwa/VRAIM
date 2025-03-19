using System.Linq;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using KosakaKen.PythonPipe.Scripts.Messages;
using KosakaKen.Utility.Scripts;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    public class AgentFacialExpression : MonoBehaviour
    {
        /// <summary>
        /// 表情を変えるアニメーションが設定されているAnimator
        /// </summary>
        [SerializeField, Required] private Animator _animator;

        /// <summary>
        /// 感情を示すAnimatorのパラメータ名
        /// </summary>
        [InfoBox("intのパラメータ\n" +
                 "0:Neutral\n" +
                 "1:Joy\n" +
                 "2:Impressed\n" +
                 "3:Convinced\n" +
                 "4:Thinking\n" +
                 "5:Sleepy\n" +
                 "6:Suspicion\n" +
                 "7:Compassion\n" +
                 "8:Embarrassing\n" +
                 "9:Anger")]
        [SerializeField, AnimatorParam("_animator")] private string _expressionParamName;
        
        private void Start()
        {
            if (_animator != null)
            {
                TryGetComponent(out _animator);
            }
            
            // シーン内からエージェントを探して、表情変化のイベントを設定する
            var agent = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IAgent>().FirstOrDefault();
            agent?.ReceivedDialogue2
                .Subscribe(message => PlayExpression(message.ExpressionType))
                .AddTo(this);
        }

        /// <summary>
        /// 指定した感情のタイプに合わせて表情を変更する
        /// </summary>
        /// <param name="expressionType"></param>
        private void PlayExpression(ExpressionType expressionType)
        {
            DebugLogger.LogEditorOnly($"[FacialExpression] ExpressionType : {expressionType}");
            _animator.SetInteger(_expressionParamName, (int)expressionType);
        }

#if UNITY_EDITOR
        public void PlayExpressionTest(ExpressionType expressionType)
        {
            _animator.SetInteger(_expressionParamName, (int)expressionType);
        }
#endif
    }
}