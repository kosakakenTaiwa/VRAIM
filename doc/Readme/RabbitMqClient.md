Remdisとの通信のやり方
===
## RabbitMQClientクラス
RabbitMQとの通信を行う機能を持つクラス。  
基本的にこのクラスからRabbitMQサーバーにアクセスする。  
シングルトンで実装されているので、このクラスにアクセスするには
```csharp
RabbitMQClient.Instance
```
とすればアクセスできる。  
このクラスから利用するのは基本的に以下の3種類
- async UniTask PublishMessageAsync(RemdisMessageDto, string , string = "", CancellationToken = default)
- async UniTask SubscribeAsync(string, Action\<RemdisMessageDto\>, CancellationToken)
- async UniTask SubscribeAsync(string, Action\<RemdisEmotionMessageDto\>, CancellationToken)

## RabbitMQサーバーに情報を送信する
RabbitMQサーバーに情報を送信するには、DTOオブジェクトを`RabbitMQClient`クラスに渡す必要がある。  
Remdisが送受信する情報は以下の形式のJSONファイルになっているので、
まずはその形式に合わせたオブジェクトのインスタンスを作成する。
```json
{
  "timestamp" : UNIX時間,
  "id" : remdisで生成されたuuid,
  "producer" : 送信者名,
  "update_type" : "add" or "commit" or "revoke",
  "exchange" : 送信プログラム名,
  "body" : メッセージの中身,
  "data_type" : 特定のデータ形式だった場合に追記される
}
```
この形式に沿ったクラスは`RemdisMessage`クラスとして定義済みなので、
インスタンスを作成すればOK。  

### メッセージを送信するスクリプトの作成例
送信するメッセージのインスタンスを作成。
```csharp
RemdisMessage message = new RemdisMessage()
    {
        timestamp = (long)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds,
        id = Guid.NewGuid().ToString(),
        producer = "",
        update_type = "",
        exchange = "",
        body = "",
        data_type = ""
    }
```

ここで作成したインスタンスと、それをどこに送信するかをRabbitMQClientに渡して送信完了。
```csharp
// 送信するオブジェクトはDTOにしなければならないのでコンストラクタを使って変換する
await RabbitMqClient.Instance.PublishMessageAsync(new RemdisMessageDto(message), _channelName, cancellationToken: _token);
or
RabbitMqClient.Instance.PublishMessageAsync(new RemdisMessageDto(message), _channelName, cancellationToken: _token).Forgot();
```

## RabbitMQサーバーから情報を受信する
情報を受信するには、情報を受信したときの動作を記述したメソッドを作る。  
メソッドの引数は`RemdisMessageDto`もしくは`RemdisEmotionMessageDto`にしておく。  
UniRxのサブスクライブとほぼ同じ感じ。

### 受信した情報をログに出力するコード例

```csharp
// RemdisMessageDtoを引数として受け取るメソッドを定義
private void Log(RemdisMessageDto message)
{
    // Remdisのメッセージをログに出力するメソッドを使用する
    RemdisLogger.Log(message);
}
```
情報を受信したときに`Log()`が呼び出されるようにする。
```csharp
private void Start()
{
    // 非同期メソッドに渡すためのCancellationTokenを作成
    // MonoBehaviourがthis.GetCancellationTokenOnDestroy()を使うことによって、
    // Destoryされたときに自動でcancellされるCancellationTokenを作成できる。
    var token = this.GetCancellationTokenOnDestroy();
    // _channelNameはどのチャンネルから情報を受信するかを指定する。
    // チャンネル名は"asr","dialogue","dialogue2","tts"などがある。後でクラスとして定義するかも。
    RabbitMqClient.Instance.SubscribeAsync(_channelName, Log, token).Forget();
}
```
ここで注意しなければならないのは、メッセージを受信した直後に処理は実行されないこと。  
処理をメインスレッドに移行する関係上、約1～2フレームの遅延が発生する可能性がある。
