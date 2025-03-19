# 利用方法

## プログラムの実行手順

1. RabbitMQサーバーを立ち上げる
2. UnityでDictationAppシーンを実行する
3. Remdisを実行
4. 対話  

という流れです。

## 詳細な手順の解説

手順の中でRemdisの実行をバッチファイルで行っていますが、あくまで簡略化のためであってRabbitMQサーバーの起動とモジュールの実行さえできれば大丈夫です。

### 1. RabbitMQサーバーを立ち上げる

これはバッチファイルが用意されているので、そちらを実行してください。  
バッチファイルは`Batch\run-RabbitMQ.bat`にあります。  
バッチファイルを実行して以下のように表示されたら成功です。

``` commandline
"現在時刻" [info] <0.633.0> Server startup complete; 4 plugins started.
"現在時刻" [info] <0.633.0>  * rabbitmq_prometheus
"現在時刻" [info] <0.633.0>  * rabbitmq_management
"現在時刻" [info] <0.633.0>  * rabbitmq_management_agent
"現在時刻" [info] <0.633.0>  * rabbitmq_web_dispatch
"現在時刻" [info] <0.9.0> Time to start RabbitMQ: 4306057 us
```

表示されない場合、Docker Desktopを立ち上げるか、1分ほど待ってからもう一度実行する。

### 2. UnityでDictationAppシーンを実行する

Unityを開き、`DictationApp_Unity`シーンを開きます。  
パスは`Assets/KosakaKen/DictationAI/Scene/DictationLoggingApp_Unity.unity`  
シーンを開いたら画面上部の再生ボタンを押して実行します。  
このとき、ヒエラルキーにあるUnitySystemsと書かれたオブジェクトをクリックすると、インスペクターに`AppManager`というコンポーネントがあり、そこの`AppMode`からVRとPCモードを切り替えることができます。  
VR機器を使わないときはこの設定を活用してください。ただし、VR専用の機能は使えなくなります。  
VRを使用する場合は、Unityの実行前にPCとHMDを接続しておきます。

### 3. Remdisを実行

Remdisを実行するには、RabbitMQサーバーと同様にバッチファイルで実行できます。  
実行するバッチファイルは`Batch/run.bat`です。
実行できない場合は、サインインしているユーザーのフォルダの下にminiconda3がインストールされているかどうか確認して、なければインストールしてください。  

バッチファイルを実行しても上手くいかない場合は、Remdisのドキュメントを参照しながらRemdisを実行してください。  
その際、起動するモジュールは`input.py`,`asr.py`,`dialogue.py`,`tts.py`の4つです。  
本研究室の環境ではVAPを使用した場合上手く動作しなかったため、VAPの起動は省略しています。

```commandline
python input.py
python asr.py
python dialogue.py
python tts.py
```

### 4. 対話

あとはUnityの画面に戻って対話するだけです。  
音声の入力がされない場合はマイクの設定を見直した後、Remdisを再起動してください。  
また、ジェスチャー認識を有効化したサンプルシーンでは最初に手を振るジェスチャーを行わないとマイクが入力されないようになっています。

## UnityとVRデバイスの接続方法

- Meta Quest
  - 詳細なやり方は[こちら](4_MetaQuest_Execute_Guide_ja.md)から

その他Steam VRなどUnityと連携できるものであれば使用することができます。
開発者は[Virtual Desktop](https://www.meta.com/experiences/2017050365004772/)を使用していました。
