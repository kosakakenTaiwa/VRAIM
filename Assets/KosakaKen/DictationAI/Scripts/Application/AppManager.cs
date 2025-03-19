using System;
using System.Linq;
using JetBrains.Annotations;
using KosakaKen.Utility.Scripts;
using UniRx;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Application
{
    [Serializable]
    public enum AppMode
    {
        PC,
        PCVR,
        VRStandalone
    }

    [Serializable]
    public class PlayerProperty
    {
        public GameObject PlayerObject => _playerObject;
        public Camera Camera => _camera;
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private Camera _camera;

        public void SetObject(GameObject playerObj, Camera camera)
        {
            _playerObject = playerObj;
            _camera = camera;
        }
    }
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                var gameObj = new GameObject("AppManager");
                _instance = gameObj.AddComponent<AppManager>();
                return _instance;
            }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _instance = value;
            }
        }

        private static AppManager _instance;

        public IObservable<AppMode> OnChangeAppMode => _appMode;
        public AppMode Mode => _appMode.Value;

        public Camera CurrentCamera
        {
            get
            {
                return _appMode.Value switch {
                    AppMode.PC => _pcPlayerProperty.Camera,
                    _ => _vrPlayerProperty.Camera
                };
            }
        }
        [SerializeField] private ReactiveProperty<AppMode> _appMode = new();
        [SerializeField] private PlayerProperty _pcPlayerProperty = new();
        [SerializeField] private PlayerProperty _vrPlayerProperty = new();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Initialize();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Initialize()
        {
            // VRとPCのオブジェクトとカメラを取得できているかをチェック
            if (_vrPlayerProperty.PlayerObject == null || _vrPlayerProperty.Camera == null ||
                _pcPlayerProperty.PlayerObject == null || _pcPlayerProperty.Camera == null)
            {
                // VRのセットアップ
                var xrOrigin = FindAnyObjectByType<XROrigin>();
                _vrPlayerProperty.SetObject(xrOrigin.gameObject, xrOrigin.Camera);
            
                // PCのセットアップ
                var cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
                foreach (var cam in cameras)
                {
                    if (cam.transform.IsChildOf(xrOrigin.transform)) continue;
                    _pcPlayerProperty.SetObject(cam.gameObject, cam);
                    break;
                }
            }
            _appMode
                .Subscribe(ModeChangeAction)
                .AddTo(this);
        }
        
        private void ModeChangeAction(AppMode mode)
        {
            DebugLogger.LogEditorOnly($"ModeChange : {mode}");
            _pcPlayerProperty.PlayerObject.SetActive(mode is AppMode.PC);
            _vrPlayerProperty.PlayerObject.SetActive(mode is AppMode.PCVR or AppMode.VRStandalone);
        }

        public void ChangeMode()
        {
            _appMode.Value = _appMode.Value switch
            {
                AppMode.PC => AppMode.PCVR,
                AppMode.PCVR => AppMode.PC,
                _ => AppMode.PC,
            };
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}