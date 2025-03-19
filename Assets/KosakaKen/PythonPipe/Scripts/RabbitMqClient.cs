using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using KosakaKen.PythonPipe.Scripts.Messages;
using KosakaKen.PythonPipe.Scripts.RemdisLog;
using KosakaKen.Utility.Scripts;
using NaughtyAttributes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UnityEngine;

[assembly:InternalsVisibleTo("EditorTests")]
namespace KosakaKen.PythonPipe.Scripts
{
    /// <summary>
    /// RabbitMQを用いたサーバー通信を行うためのシングルトン。
    /// サーバーとの確実な接続と切断を行うためにシングルトンにしている。
    /// チャンネルの取得はここから行う。
    /// </summary>
    [RequireComponent(typeof(UnityMainThreadDispatcher))]
    public sealed class RabbitMqClient : MonoBehaviour
    {
        private enum RabbitMQLogMode
        {
            None,
            OnlySubscribe,
            OnlyPublish,
            Full
        }

        public enum LogTarget
        {
            none = 0,
            asr = 1 << 0,
            dialogue = 1 << 2,
            tts = 1 << 3,
            vap = 1 << 4,
            dialogue2 = 1 << 5,
        }
        
        /// <summary>
        /// シングルトン
        /// </summary>
        public static RabbitMqClient Instance;
        /// <summary>
        /// ホスト名
        /// </summary>
        public string HostName => _hostName;
        /// <summary>
        /// 接続しているかどうか
        /// </summary>
        public bool IsOpen => _connection.IsOpen;
        
        [SerializeField] private string _hostName = "localhost";
        [SerializeField] private RabbitMQLogMode _logMode;
        [SerializeField, EnumFlags] private LogTarget _logTarget;
        private IConnection _connection;
        private Dictionary<string, IModel> _modelDictionary = new();
        private CancellationToken _token;

        private async void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _token = this.GetCancellationTokenOnDestroy();
                DontDestroyOnLoad(gameObject);
                await ConnectServer(_token);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// サーバーとの接続を開始させる
        /// </summary>
        public async UniTask ConnectServer(CancellationToken token)
        {
            // 接続済みなら処理を中断
            if (_connection != null)
            {
                DebugLogger.LogEditorOnly("既にサーバーとの接続を行っています。", LogType.Warning);
                return;
            }
            
            var cf = new ConnectionFactory(){ HostName = _hostName };
            while (_connection is not { IsOpen: true })
            {
                DebugLogger.LogEditorOnly("サーバーとの接続を試みています...");
                _connection = cf.CreateConnection();
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken:token);
            }
            DebugLogger.LogEditorOnly($"サーバーとの接続に成功しました。 isOpen:{_connection.IsOpen}");
        }

        /// <summary>
        /// 特定のチャンネルを取得する
        /// </summary>
        /// <param name="channelName">チャンネル名</param>
        /// <param name="token">Cancellation Token</param>
        /// <returns></returns>
        public async UniTask<IModel> GetChannel(string channelName, CancellationToken token)
        {
            await UniTask.WaitUntil(() =>
            {
                DebugLogger.LogEditorOnly($"サーバーとの接続が確立するのを待っています... isOpen:{_connection is { IsOpen: true }}");
                return _connection is { IsOpen: true };
            }, cancellationToken: token);
            
            if (!_modelDictionary.ContainsKey(channelName))
            {
                var model = _connection.CreateModel();
                model.ExchangeDeclare(channelName, ExchangeType.Fanout);
                _modelDictionary.Add(channelName, model);
            }
            return _modelDictionary.GetValueOrDefault(channelName);
        }

        /// <summary>
        /// 特定のチャンネルの接続を切断
        /// </summary>
        /// <param name="exchangeName"></param>
        public void CloseChannel(string exchangeName)
        {
            if (!_modelDictionary.ContainsKey(exchangeName)) return;
            // 接続の中断
            _modelDictionary.GetValueOrDefault(exchangeName).Close();
            _modelDictionary.Remove(exchangeName);
        }
        
        /// <summary>
        /// RabbitMQサーバーにメッセージを送信する。
        /// 送信するメッセージの形式はRemdisによって決定される。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channelName"></param>
        /// <param name="routingKey"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask PublishMessageAsync(RemdisMessageDto message, string channelName, string routingKey = "", CancellationToken cancellationToken = default)
        {
            var model = await GetChannel(channelName, cancellationToken);
            await UniTask.RunOnThreadPool(() =>
                {
                    try
                    {
                        if (model.IsClosed) return;
                        var body = JsonUtility.ToJson(new RemdisMessage(message));
                        model.BasicPublish(channelName, routingKey, null, Encoding.UTF8.GetBytes(body));
                        if (_logMode is RabbitMQLogMode.OnlyPublish or RabbitMQLogMode.Full && IsLogTarget(message.exchange, _logTarget))
                        {
                            RemdisLogger.Log(message);   
                        }
                    }
                    catch (Exception e)
                    {
                        DebugLogger.LogEditorOnly($"[RabbitMQ Client] failer send. reason: {e.Message}");
                    }
                },
                cancellationToken:cancellationToken);
        }

        /// <summary>
        /// 接続の切断
        /// </summary>
        public void Disconnect()
        {
            // _connectionがnullならそもそも接続していないのでスキップ
            if (_connection == null) return;
            
            // 初期化
            _connection.Close();
            _connection = null;
            foreach(var model in _modelDictionary.Values)
            {
                model.Close();
            }
            _modelDictionary.Clear();
            DebugLogger.LogEditorOnly("[RabbitMQ Client] サーバーとの通信の切断を正常に完了しました。");
        }

        /// <summary>
        /// Remdisからのメッセージを受信したときの動作を設定する
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public async UniTask SubscribeAsync(string channelName, Action<RemdisMessageDto> action, CancellationToken token)
        {
            var model = await GetChannel(channelName, token);
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (_, ea) =>
            {
                var dto = RemdisMessageConverter.ToRemdisMessage(ea);
                // 条件に合致していればログを出力する
                if (_logMode is RabbitMQLogMode.OnlySubscribe or RabbitMQLogMode.Full && IsLogTarget(dto.exchange, _logTarget))
                {
                    UnityMainThreadDispatcher.instance?.Enqueue(() => RemdisLogger.Log(dto, new RemdisLogOption()));
                }
                // consumerのイベント発火は非メインスレッドから呼び出される。
                // UnityのAPIを使うためにはメインスレッドからの呼び出しに切り替えなければならないので、
                // メインスレッドに処理を渡すクラスを介して処理を実行する。
                UnityMainThreadDispatcher.instance?.Enqueue(() => action?.Invoke(dto));
            };
            // キューのバインド
            var queueName = model.QueueDeclare().QueueName;
            model.QueueBind(queue:queueName, exchange:channelName, routingKey:"");
            // メッセージの受信を開始する
            model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
        
        /// <summary>
        /// 感情メッセージを受信したときの動作を設定する
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public async UniTask SubscribeAsync(string channelName, Action<RemdisEmotionMessageDto> action, CancellationToken token)
        {
            var model = await GetChannel(channelName, token);
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (_, ea) =>
            {
                var dto = RemdisMessageConverter.ToRemdisEmotionMessage(ea);
                // 条件に合致していればログを出力する
                if (_logMode is RabbitMQLogMode.OnlySubscribe or RabbitMQLogMode.Full && IsLogTarget(dto.Exchange, _logTarget))
                {
                    UnityMainThreadDispatcher.instance?.Enqueue(() => RemdisLogger.Log(dto, new RemdisLogOption()));
                }
                // consumerのイベント発火は非メインスレッドから呼び出される。
                // UnityのAPIを使うためにはメインスレッドからの呼び出しに切り替えなければならないので、
                // メインスレッドに処理を渡すクラスを介して処理を実行する。
                UnityMainThreadDispatcher.instance?.Enqueue(() => action?.Invoke(dto));
            };
            // キューのバインド
            var queueName = model.QueueDeclare().QueueName;
            model.QueueBind(queue:queueName, exchange:channelName, routingKey:"");
            // メッセージの受信を開始する
            model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private void OnDestroy()
        {
            Disconnect();
            if (Instance == this)
            {
                Instance = null;
            }
        }
        
        internal static bool IsLogTarget(string exchangeName, LogTarget targetSetting)
        {
            return exchangeName switch
            {
                "asr" => (LogTarget.asr & targetSetting) != 0,
                "tts" => (LogTarget.tts & targetSetting) != 0,
                "dialogue" => (LogTarget.dialogue & targetSetting) != 0,
                "vap" => (LogTarget.vap & targetSetting) != 0,
                "dialogue2" => (LogTarget.dialogue2 & targetSetting) != 0,
                _ => false
            };
        }
    }
}