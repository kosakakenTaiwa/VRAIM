using System;
using KosakaKen.DictationAI.Scripts.Client.Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace KosakaKen.DictationAI.Scripts.Client
{
    public class ClientControllerInput : MonoBehaviour
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static ClientControllerInput Instance;
        /// <summary>
        /// プレイヤーの入力イベント
        /// </summary>
        public IPlayerInputEvents PlayerInputEvents => _playerInput;
        /// <summary>
        /// 左手のコントローラー
        /// </summary>
        public ActionBasedController LeftController => _leftController;
        /// <summary>
        /// 右手のコントローラー
        /// </summary>
        public ActionBasedController RightController => _rightController;
        /// <summary>
        /// 利き手
        /// </summary>
        public PriorityHandSide HandSide => _priorityHandSide;
        [InfoBox("Controllerは自動で取得されます。")]
        [SerializeField] private ActionBasedController _leftController;
        [SerializeField] private ActionBasedController _rightController;
        private PlayerInput _playerInput;

        public enum PriorityHandSide
        {
            Left,
            Right
        }
        [SerializeField] private PriorityHandSide _priorityHandSide = PriorityHandSide.Right;

        /// <summary>
        /// 利き手のコントローラーの情報を取得する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ActionBasedController GetPriorityHandController()
        {
            return  _priorityHandSide switch {
                PriorityHandSide.Left => _leftController,
                PriorityHandSide.Right => _rightController,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// 利き手を変更する。
        /// 右→左、左→右
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangePriorityHandController()
        {
            _priorityHandSide = _priorityHandSide switch {
                PriorityHandSide.Left => PriorityHandSide.Right,
                PriorityHandSide.Right => PriorityHandSide.Left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// 指定したほうの手を利き手にする
        /// </summary>
        /// <param name="priorityHandSide"></param>
        public void ChangePriorityHandController(PriorityHandSide priorityHandSide)
        {
            _priorityHandSide = priorityHandSide;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _playerInput = new PlayerInput();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            // 左右どちらかのコントローラーがセットされていない場合は見つけ出してセットする
            if (_leftController != null && _rightController != null) return;
            
            var xrInputModalityManager = FindAnyObjectByType<XRInputModalityManager>();
            if (_leftController == null)
                xrInputModalityManager.leftController.TryGetComponent(out _leftController);
            if (_rightController == null)
                xrInputModalityManager.rightController.TryGetComponent(out _rightController);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}