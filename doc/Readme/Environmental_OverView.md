# 開発環境について

## Unityのプロジェクト設定

- バージョン
  - Unity 2022.3.7f1
- レンダーパイプライン
  - Universal Render Pipeline
- ターゲットプラットフォーム
  - Windows
  - OpenXR経由でのPCVR想定
- 入力システム
  - InputSystemのみ
  - InputManagerは無効化

## Unity内で使っているライブラリ

以下のライブラリはプロジェクト初回起動時に自動でインストールされます。

- [XR Interaction Toolkit v2.4.3](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.4/manual/index.html)
- [Unity Toon Shader v0.10](https://docs.unity3d.com/Packages/com.unity.toonshader@0.10/manual/index.html)
- [UniTask v2.5.4](https://github.com/Cysharp/UniTask)
- [UniRx v7.1.0](https://github.com/neuecc/UniRx)
- [uLipSync v3.1.1](https://github.com/hecomi/uLipSync)
- [Naughty Attribute v2.1.4](https://github.com/dbrizov/NaughtyAttributes)
- [NuGetForUnity v4.1.0](https://github.com/GlitchEnzo/NuGetForUnity)
- [JetBrains RiderFlow v2023.1.0-25](https://www.jetbrains.com/ja-jp/riderflow/)

その他、既にインストールされているパッケージは以下の通りです。
全てApache2.0もしくはMIT Licenseで提供されています。
それぞれどのライセンスで提供されているかはUnityのNuGet -> Manage Nuget Packageから確認できます。
- [RabbitMQ.Client v6.8.1](https://www.nuget.org/packages/RabbitMQ.Client)
- [NAudio v2.2.1](https://github.com/naudio/NAudio)
- [NAudio.Core v2.2.1](https://github.com/naudio/NAudio)
- [NAudio.Wasapi v2.2.1](https://github.com/naudio/NAudio)
- [NAudio.WinMM v2.2.1](https://github.com/naudio/NAudio)
- [System.Runtime.CompilerServices.Unsafe v6.0.0](https://dotnet.microsoft.com)
- [System.Threading.Channels v7.0.0](https://dotnet.microsoft.com)
