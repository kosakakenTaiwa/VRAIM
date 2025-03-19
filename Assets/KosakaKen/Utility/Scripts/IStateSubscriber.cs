using System;
using UniRx;

namespace KosakaKen.Utility.Scripts
{
    public interface IStateSubscriber<out TState> where TState : struct, Enum
    {
        /// <summary>
        /// 状態名
        /// </summary>
        public TState State { get; }
        /// <summary>
        /// 状態になった瞬間に呼び出される処理
        /// </summary>
        public IObservable<Unit> OnEnter { get; }
    
        /// <summary>
        /// 状態から抜ける瞬間に実行される処理
        /// </summary>
        public IObservable<Unit> OnExit { get; }
    
        /// <summary>
        /// 毎フレーム実行される処理. deltaTimeが渡される。
        /// </summary>
        public IObservable<float> OnUpdate { get; }
    }
}