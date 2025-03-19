using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KosakaKen.Utility.Scripts
{
    [Serializable]
    public abstract class StateMachineControllerBase<TState, TStateTrigger>
        : IStateMachineController<TStateTrigger>
        where TState : struct, Enum, IComparable
        where TStateTrigger : struct, Enum, IComparable
    {
        /// <summary>
        /// ステートマシンの初期状態
        /// </summary>
        [SerializeField] protected TState _initialState;
        
        /// <summary>
        /// ステートマシンがとる各状態を定義する
        /// </summary>
        [SerializeField] protected List<StateMap<TState>> _stateMaps = new();
        
        /// <summary>
        /// ステートマシン
        /// </summary>
        protected StateMachine<TState, TStateTrigger> _stateMachine;
        
        /// <summary>
        /// 初期化処理を実行した後か
        /// </summary>
        protected bool _isInitialized = false;

        /// <summary>
        /// 初期化処理。各メソッドを呼び出す前にこれを呼び出す。
        /// </summary>
        /// <param name="monoBehaviour"></param>
        public virtual void Initialize(MonoBehaviour monoBehaviour)
        {
            _stateMachine = new StateMachine<TState, TStateTrigger>(monoBehaviour, _initialState);
            SetUpState();
            SetUpTransition();
            _isInitialized = true;
        }

        /// <summary>
        /// 各状態を定義する。
        /// Initialize()を実行したときにこれも呼び出される。
        /// </summary>
        protected virtual void SetUpState()
        {
            foreach (var stateMap in _stateMaps)
            {
                _stateMachine.SetupState(stateMap.State, onEnter:stateMap.InvokeOnEnter, onExit:stateMap.InvokeOnExit, onUpdate:stateMap.InvokeOnUpdate);
            }
        }

        /// <summary>
        /// 遷移条件を設定する。
        /// Initialize()を実行したときにこれも呼び出される。
        /// </summary>
        protected abstract void SetUpTransition();
        
        /// <summary>
        /// 指定したステートのイベント購読用インターフェースを取得する
        /// </summary>
        /// <param name="targetState">取得したいステート</param>
        /// <returns>イベント購読用インターフェース</returns>
        /// <exception cref="ArgumentException">指定したステートがステートマップで定義されていないものだったとき</exception>
        public virtual IStateSubscriber<TState> GetStateSubscriber(TState targetState)
        {
            // 初期化しているか条件を確認する。
            if (!_isInitialized) throw new Exception("Call the initialization method before calling this method.");
            
            foreach (var stateMap in _stateMaps.Where(stateMap => stateMap.State.Equals(targetState)))
            {
                return stateMap;
            }
            throw new ArgumentException($"{targetState} is not defined. Check to see if the state is included in state map list.");
        }

        /// <summary>
        /// 毎フレーム実行する処理
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Update(float deltaTime)
        {
            // 初期化しているか条件を確認する。
            if (!_isInitialized) return;
            
            _stateMachine.Update(deltaTime);
        }

        public virtual void TryTransitionState(TStateTrigger trigger)
        {
            // 初期化しているか条件を確認する。
            if (!_isInitialized) return;
            
            _stateMachine.ExecuteTrigger(trigger);
        }
    }
}