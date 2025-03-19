using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace KosakaKen.PythonPipe.Scripts
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        public static UnityMainThreadDispatcher instance { get; private set; }
        private static Queue<Action> executionQueue = new Queue<Action>();
        private static int _mainThreadId;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                lock (executionQueue)
                {
                    executionQueue = new Queue<Action>();
                }
                _mainThreadId = Thread.CurrentThread.ManagedThreadId;
            }
            else
            {
                Destroy(this);
            }
        }

        public static bool IsMainThread(int threadId)
        {
            return threadId == _mainThreadId;
        }

        public void Enqueue(Action action)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(action);
            }
        }

        void Update()
        {
            if (executionQueue.Count <= 0) return;
            lock (executionQueue)
            {
                while (executionQueue.Count > 0)
                {
                    executionQueue.Dequeue()?.Invoke();
                }
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}