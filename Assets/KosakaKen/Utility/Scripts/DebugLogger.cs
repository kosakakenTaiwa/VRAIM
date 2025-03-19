using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace KosakaKen.Utility.Scripts
{
    /// <summary>
    /// ビルドデータでのDebug.Logの呼び出しを避けるためのDebugクラス
    /// </summary>
    /// 参考リンク：https://qiita.com/toRisouP/items/d856d65dcc44916c487d
    public static class DebugLogger
    {
        /// <summary>
        /// Editorでの再生時のみログを出力するクラス
        /// </summary>
        /// <param name="t">ログに出力するテキスト</param>
        /// <param name="logType">ログの出力形式</param>
        /// <exception cref="ArgumentOutOfRangeException">ログの出力形式が非対応のものを指定した場合</exception>
        [Conditional("UNITY_EDITOR")]
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public static void LogEditorOnly(string t, LogType logType = LogType.Log)
        {
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(t);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(t);
                    break;
                case LogType.Error:
                    Debug.LogError(t);
                    break;
                case LogType.Assert:
                case LogType.Exception:
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        /// <summary>
        /// Editorでの再生時にLogExceptionを出力する
        /// </summary>
        /// <param name="e">何らかのException</param>
        [Conditional("UNITY_EDITOR")]
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public static void LogEditorOnly(Exception e)
        {
            Debug.LogException(e);
        }

        /// <summary>
        /// Editorでの再生時にアサーションをログとして出力するとき
        /// </summary>
        /// <param name="condition">アサーションを通すときの条件</param>
        /// <param name="t">アサーションが通らなかったときに出力するテキスト</param>
        [Conditional("UNITY_EDITOR")]
        public static void AssertEditorOnly(bool condition, string t = "")
        {
            Debug.Assert(condition, t);
        }
    }
}