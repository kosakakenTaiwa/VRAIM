using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
namespace KosakaKen.Utility.Scripts
{
    [Serializable]
    public class StateMap<TState> : IStateSubscriber<TState> where TState : struct, Enum
    {
        public TState State => _state;
        public IObservable<Unit> OnEnter => _onEnterEvent;
        public IObservable<Unit> OnExit => _onExitEvent;
        public IObservable<float> OnUpdate => _onUpdateEvent;

        [SerializeField] private TState _state;
        [SerializeField] private UnityEvent _onEnterUnityEvent = new();
        [SerializeField] private UnityEvent _onExitUnityEvent = new();
        [SerializeField] private UnityEvent<float> _onUpdateUnityEvent = new();
        private readonly Subject<Unit> _onEnterEvent = new();
        private readonly Subject<Unit> _onExitEvent = new();
        private readonly Subject<float> _onUpdateEvent = new();

        /// <summary>
        /// Enter処理を呼び出す
        /// </summary>
        public void InvokeOnEnter()
        {
            DebugLogger.LogEditorOnly($"<b><color=aqua>[StateMachine]</color></b> :: Enter : {State}");
            _onEnterEvent.OnNext(Unit.Default);
            _onEnterUnityEvent?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Exit処理を呼び出す
        /// </summary>
        public void InvokeOnExit()
        {
            DebugLogger.LogEditorOnly($"<b><color=aqua>[StateMachine]</color></b> :: Exit : {State}");
            _onExitEvent.OnNext(Unit.Default);
            _onExitUnityEvent?.Invoke();
        }

        /// <summary>
        /// Update処理を呼び出す
        /// </summary>
        public void InvokeOnUpdate(float deltaTime)
        {
            DebugLogger.LogEditorOnly($"<b><color=aqua>[StateMachine]</color></b> :: Update : {State}");
            _onUpdateEvent.OnNext(deltaTime);
            _onUpdateUnityEvent?.Invoke(deltaTime);
        }
    }
}