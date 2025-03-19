using System.Linq;
using KosakaKen.DictationAI.Scripts.Agent.Interfaces;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KosakaKen.DictationAI.Scripts.UI
{
    /// <summary>
    /// エージェントの身長を変更するためのコンポーネント。
    /// MVPパターンのPresenterに該当。
    /// </summary>
    public class AgentHeightAdjustPresenter : MonoBehaviour
    {
        // UnityのUIコンポーネントをViewとして見るため、Viewは作らない
        [SerializeField, Required] private TextMeshProUGUI _text;
        [SerializeField, Required] private Slider _adjustSlider;
        /// <summary>
        /// テキストに表示させる桁数
        /// </summary>
        [SerializeField] private int _digits = 2;
        private IAgentData _agentData;
        
        private void Start()
        {
            _agentData = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IAgentData>().FirstOrDefault();
            _adjustSlider.value = _agentData?.AgentHeight ?? 1f;
            _text.text = $"Agent Height : {_agentData?.AgentHeight ?? 1f}";
            
            _adjustSlider
                .ObserveEveryValueChanged(slider => slider.value)
                .Subscribe(heightValue =>
                {
                    // エージェントの身長を変更する
                    _agentData.ChangeHeight(heightValue);
                    var carry = Mathf.Pow(10, _digits);
                    var viewValue = Mathf.Round(heightValue * carry) / carry;
                    _text.text = $"Agent Height : {viewValue}";
                }).AddTo(this);
        }
    }
}