using System;
using UniRx;

namespace KosakaKen.DictationAI.Scripts.Client.Interfaces
{
    /// <summary>
    /// プレイヤーの入力イベントを取得するためのインターフェース
    /// </summary>
    public interface IPlayerInputEvents
    {
        /// <summary>
        /// 左手のトリガーボタンが押された瞬間に発火されるイベント
        /// </summary>
        IObservable<Unit> OnPressedLeftTrigger { get; }
        /// <summary>
        /// 右手のトリガーボタンが押された瞬間に発火されるイベント
        /// </summary>
        IObservable<Unit> OnPressedRightTrigger { get; }
        /// <summary>
        /// 左手のトリガーボタンが離された瞬間に発火されるイベント
        /// </summary>
        IObservable<Unit> OnReleasedLeftTrigger { get; }
        /// <summary>
        /// 左手のトリガーボタンが離された瞬間に発火されるイベント
        /// </summary>
        IObservable<Unit> OnReleasedRightTrigger { get; }
    }
}