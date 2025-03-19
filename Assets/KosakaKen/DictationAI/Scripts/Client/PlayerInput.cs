using System;
using KosakaKen.DictationAI.Scripts.Client.Interfaces;
using UniRx;

namespace KosakaKen.DictationAI.Scripts.Client
{
    public class PlayerInput : IPlayerInputEvents
    {
        public IObservable<Unit> OnPressedLeftTrigger => _onPressedLeftTrigger;
        public IObservable<Unit> OnPressedRightTrigger => _onPressedRightTrigger;
        public IObservable<Unit> OnReleasedLeftTrigger => _onReleasedLeftTrigger;
        public IObservable<Unit> OnReleasedRightTrigger => _onReleasedRightTrigger;
        private readonly Subject<Unit> _onPressedLeftTrigger = new();
        private readonly Subject<Unit> _onPressedRightTrigger = new();
        private readonly Subject<Unit> _onReleasedLeftTrigger = new();
        private readonly Subject<Unit> _onReleasedRightTrigger = new();

        public PlayerInput()
        {
            // MainInputを有効化して入力の受け取り
            var mainInput = new MainInput();
            mainInput.Enable();

            // 各種入力時のイベント発行
            mainInput.ControllerButton.LeftTrigger.started += _ =>  _onPressedLeftTrigger.OnNext(Unit.Default);
            mainInput.ControllerButton.RightTrigger.started += _ =>  _onPressedRightTrigger.OnNext(Unit.Default);
            mainInput.ControllerButton.LeftTrigger.canceled += _ => _onReleasedLeftTrigger.OnNext(Unit.Default);
            mainInput.ControllerButton.RightTrigger.canceled += _ => _onReleasedRightTrigger.OnNext(Unit.Default);
        }
    }
}