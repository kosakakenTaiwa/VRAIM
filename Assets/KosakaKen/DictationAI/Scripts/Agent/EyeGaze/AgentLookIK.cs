using KosakaKen.DictationAI.Scripts.Application;
using UniRx;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Agent.EyeGaze
{
    public class AgentLookIK : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isLookCamera = true;
        private Transform _cameraTransform;

        private void Start()
        {
            if (_animator is null) TryGetComponent(out _animator);
            if (Camera.main != null) _cameraTransform = Camera.main.transform;

            AppManager.Instance?
                .OnChangeAppMode
                .Subscribe(_ => _cameraTransform = AppManager.Instance.CurrentCamera.transform)
                .AddTo(this);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator) return;
            if (!_isLookCamera) return;
            if (_cameraTransform is null) return;
            
            _animator.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
            _animator.SetLookAtPosition(_cameraTransform.position);
        }

        /// <summary>
        /// LookIKを有効化する
        /// </summary>
        public void EnableLookIK()
        {
            _isLookCamera = true;
        }

        /// <summary>
        /// LookIKを無効化する
        /// </summary>
        public void DisableLookIK()
        {
            _isLookCamera = false;
        }
    }
}