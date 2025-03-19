# 環境構築

## 必要なソフトウェアのインストール

以下のソフトウェアのインストールとセットアップまでを行う。

- **Unity Hub のインストール**
  - [ダウンロードリンク](https://unity.com/ja/download)
- **Unity2022.3.7f1のダウンロード**
  - [Unity Archives](https://unity.com/ja/releases/editor/archive)から、該当のバージョンを探してINSTALLと書かれた青色のボタンをクリックしてインストールする。
- Oculus(Meta) のデスクトップアプリをインストール（MetaLinkを使う場合）
  - [ダウンロードリンク](https://www.meta.com/ja-jp/help/quest/articles/headsets-and-accessories/oculus-rift-s/install-app-for-link/)
- Steamのインストール（SteamVRを使う場合）
  - [ダウンロードリンク](https://store.steampowered.com/about/)
- SteamVRのインストール（SteamVRを使う場合）
  - [ダウンロードリンク](https://store.steampowered.com/app/250820/SteamVR/?l=japanese)
- **Python仮想環境のインストール（Remdisの環境構築に使えるもの）**
  - [Miniconda](https://docs.anaconda.com/miniconda/)(開発者はMinicondaを使用しています。)
  - venv など
- **Docker Desktopのインストール**  
  - Windows : [ダウンロードリンク](https://docs.docker.com/desktop/install/windows-install/)
  - その他 ： [ダウンロードリンク](https://www.docker.com/ja-jp/products/docker-desktop/)

## 初期設定

1. **本リポジトリのクローン**
2. **Remdisの環境構築**
   - Remdisは既に本リポジトリのRemdisフォルダにインストールされているのでそちらを使って環境構築してください。
   - Remdisの環境構築は、[RemdisのREADME](https://github.com/remdis/remdis?tab=readme-ov-file#step-3-%E5%90%84%E7%A8%AEapi%E9%8D%B5%E3%81%AE%E5%8F%96%E5%BE%97%E3%81%A8%E8%A8%AD%E5%AE%9A)を参照
   - 学習済みモデルは本リポジトリに含まれていないので個別にダウンロードしてください。
3. **クローンした本プロジェクトをUnityで開く**  
4. **`Assets/KosakaKen/DictationAI/Scene/DictationLoggingApp_YoneyamaAI.unity`を開き、再生ボタンを押す**  
  RabbitMQサーバーにアクセスできないといった内容のエラーが1つだけ出力されるかと思いますが、それ以外でエラーが出ていなければOKです。確認できたら再生ボタンをもう一度押してPlayモードを終了してください。
5. **バッチファイルの設定**  
   `Batch`フォルダにあるバッチファイルを開いて編集する。VRの動作に必要なのは`run.bat`だけなので、必要なければそれだけ修正すればOKです。  
   以下の部分に環境変数としてそれぞれのパスが記述されているので、自身の環境に合わせて変更してください。  

   ```commandline
   REM このバッチファイルはminicondaを使用する想定で記述されています
   
   REM minicondaの実行ファイルまでのパス
   REM 現在ログインしているユーザーフォルダからの相対パスを記述する
   set CONDA_PATH=\Miniconda3\Scripts\activate.bat
   
   REM Remdisを動作させる環境名
   set CONDA_ENV=remdis

   REM Remdis内のmodulesまでのパスを記述
   set MODULE_PATH=..\Remdis\modules

   REM Condaの起動とConda環境のアクティベート、Remdisのプログラムファイルがあるフォルダへの移動を行うコマンド
   set COMMON_CMD="%USERPROFILE%%CONDA_PATH% && conda activate %CONDA_ENV% && cd %MODULE_PATH%"
   ```
6. **アニメーションの割り当て**

   別ドキュメントにまとめています。[こちら](5_Animation_Setting_ja.md)を参照してください。

## ジェスチャー認識機能のセットアップ

ジェスチャー認識については別でセットアップが必要です。
[こちら](3_Active_Gesture_Recognition_ja.md)を参照してください。

## 実行方法

[こちらを参照](1_execute-guide_ja.md)

## 開発で役立つドキュメント

プログラムの簡単な解説などを記載したドキュメントです。

- [Remdisとの通信のやり方](RabbitMqClient.md)
- [システムの設計](SystemOverView_ja.md)
