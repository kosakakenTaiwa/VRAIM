using System;
using UnityEngine;

namespace KosakaKen.Utility.Scripts
{
    public interface IStateMachineController<TStateTrigger>
        where TStateTrigger : struct, Enum, IComparable
    {
        public void TryTransitionState(TStateTrigger trigger);
    }
}