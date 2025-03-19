using System.Linq;
using UniRx;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent
{
    /// <summary>
    /// エージェントの身長を変更するためのコンポーネント
    /// </summary>
    public class AgentHeightAdjust : MonoBehaviour
    {
        /// <summary>
        /// エージェントのルートオブジェクト
        /// </summary>
        [SerializeField] private Transform _agentObject;
        /// <summary>
        /// エージェントの情報を保持したオブジェクト
        /// </summary>
        private IAgentData _agentData;

        private void Start()
        {
            _agentData = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IAgentData>().FirstOrDefault();
            _agentData?.OnChangeAgentHeight
                .Subscribe(AdjustHeight)
                .AddTo(this);
        }

        private void AdjustHeight(float height)
        {
            _agentObject.localScale = new Vector3(height, height, height);
        }
    }
}