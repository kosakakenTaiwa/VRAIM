using System;
using System.IO;
using KosakaKen.Utility.Scripts;
using UnityEngine;

namespace KosakaKen.PythonPipe.Scripts.Audio
{
    /// <summary>
    /// 音声データをUnityで扱うための様々なメソッドを持ったクラス。
    /// </summary>
    public static class AudioConvert
    {   
        /// <summary>
        /// Base64でエンコードされた音声データからAudioClipを生成する。
        /// </summary>
        /// <param name="base64Audio">Base64でエンコードされた音声データ</param>
        /// <param name="audioClipName">AudioClipの名前</param>
        /// <param name="sampleRate">サンプリングレート。単位はHz</param>
        /// <param name="channels">チャンネル数</param>
        /// <param name="bitPerSample">ビットレート。単位はbit</param>
        /// <returns></returns>
        public static AudioClip ToAudioClipFromBase64(string base64Audio, string audioClipName, int sampleRate = 16000, short channels = 1, short bitPerSample = 16)
        {
            var audioData = Convert.FromBase64String(base64Audio);
            return ToAudioClipFromByteArray(audioData, audioClipName, sampleRate, channels, bitPerSample);
        }
        
        /// <summary>
        /// バイト列のデータからAudioClipを生成する。
        /// </summary>
        /// <param name="rawPCMData">rawPCM形式の音声データ</param>
        /// <param name="audioClipName">AudioClipの名前</param>
        /// <param name="sampleRate">サンプリングレート。単位はHz</param>
        /// <param name="channels">チャンネル数</param>
        /// <param name="bitsPerSample">ビットレート。単位はbit</param>
        /// <returns></returns>
        public static AudioClip ToAudioClipFromByteArray(byte[] rawPCMData, string audioClipName, int sampleRate = 16000, short channels = 1, short bitsPerSample = 16)
        {
            var audioData = ConvertRawPCMToFloatArray(rawPCMData, bitsPerSample, channels);
            // オーディオデータが空の場合、AudioClipに入れられないので例外を出す。
            if (audioData.Length is 0) throw new ArgumentException("rawPCMData Length is 0! It is not allowed!");
            var audioClip = AudioClip.Create(audioClipName, audioData.Length / channels, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);
            return audioClip;
        }

        /// <summary>
        /// RawPCMデータをfloatの配列に変換する処理
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="bitDepth"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float[] ConvertRawPCMToFloatArray(byte[] rawData, int bitDepth, int channels)
        {
            int totalSamples = rawData.Length / (bitDepth / 8);
            float[] floatArray = new float[totalSamples];

            if (bitDepth == 16)
            {
                for (int i = 0; i < totalSamples; i++)
                {
                    // i * 2 バイトから始まる配列から16bitを取り出してInt16に変換
                    short sample = (short)((rawData[i * 2 + 1] << 8) | rawData[i * 2]);
                    floatArray[i] = sample / 32768.0f;
                }
            }
            else if (bitDepth == 8)
            {
                for (int i = 0; i < totalSamples; i++)
                {
                    byte sample = rawData[i];
                    floatArray[i] = (sample - 128) / 128.0f;
                }
            }
            else
            {
                DebugLogger.LogEditorOnly(new Exception("Unsupported bit depth. bit depth should be 8 or 16."));
            }

            return floatArray;
        }

        /// <summary>
        /// バイト列を浮動小数点数の配列に変換する。
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="bitPerSample">ビットレート。単位はbit</param>
        /// <returns>バイト列をfloat配列に変換したもの</returns>
        [Obsolete("このコードは信頼性が不確かです。rawPCMデータを扱う場合、ConvertRawPCMToFloatArrayを使用してください。")]
        public static float[] ConvertToFloatArray(byte[] byteArray, short bitPerSample = 16)
        {
            int sampleCount = byteArray.Length / (bitPerSample / 8); // 1バイトは8ビットなので8で割る
            var floatArray = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                var sample = BitConverter.ToInt16(byteArray, i * (bitPerSample / 8));
                floatArray[i] = sample / 32768.0f; // 16ビットPCMの範囲をfloatの範囲に正規化
            }
            return floatArray;
        }

        /// <summary>
        /// float[]からオーディオクリップを作成する。
        /// </summary>
        /// <param name="audioData"></param>
        /// <param name="sampleRate"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        public static AudioClip CreateAudioClipFromFloatArray(float[] audioData, int sampleRate, int channels)
        {
            var audioClip = AudioClip.Create("DecodedClip", audioData.Length / channels, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);
            return audioClip;
        }
        
        /// <summary>
        /// オーディオデータをWAVファイルとして保存する。
        /// デバッグ用
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="audioData">ヘッダー情報なしのバイト列</param>
        /// <param name="sampleRate">音声のサンプリングレート。単位はHz</param>
        /// <param name="channels">音声のチャンネル数</param>
        /// <param name="bitsPerSample">ビットレート。単位はbit。</param>
        public static void SaveWav(string filename, byte[] audioData, int sampleRate = 16000, short channels = 1, short bitsPerSample = 16)
        {
            using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            using (var writer = new BinaryWriter(fs))
            {
                // WAVファイルヘッダーの書き込み
                writer.Write(new char[] { 'R', 'I', 'F', 'F' });
                writer.Write(36 + audioData.Length); // ファイルサイズ - 8
                writer.Write(new char[] { 'W', 'A', 'V', 'E' });
                writer.Write(new char[] { 'f', 'm', 't', ' ' });
                writer.Write(16); // fmtチャンクのサイズ
                writer.Write((short)1); // フォーマットコード（1 = PCM）
                writer.Write(channels);
                writer.Write(sampleRate);
                writer.Write(sampleRate * channels * bitsPerSample / 8); // バイト率
                writer.Write((short)(channels * bitsPerSample / 8)); // ブロックアライン
                writer.Write(bitsPerSample);

                // データチャンクの書き込み
                writer.Write(new char[] { 'd', 'a', 't', 'a' });
                writer.Write(audioData.Length);
                writer.Write(audioData);
            }
        }
    }
}