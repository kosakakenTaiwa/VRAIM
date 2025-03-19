using System;

namespace KosakaKen.PythonPipe.Scripts.Messages
{
    [Serializable]
    public class RemdisEmotionMessage
    {
        /// <summary>
        /// メッセージが送られた時間。UNIX時間で表現される。
        /// </summary>
        public long timestamp;
        
        /// <summary>
        /// 何かのID。会話内容を溜めておいて、後で参照するときに必要そう。
        /// </summary>
        public string id;
        
        /// <summary>
        /// メッセージの送信者。
        /// </summary>
        public string producer;
        
        /// <summary>
        /// Remdisで使われるメッセージのタイプ的なもの
        /// </summary>
        public string update_type;
        
        /// <summary>
        /// 送信するルート名
        /// </summary>
        public string exchange;
        
        /// <summary>
        /// メッセージの中身。
        /// </summary>
        public EmotionBody body;

        /// <summary>
        /// 送信されたデータの型
        /// </summary>
        public string data_type;

        public RemdisEmotionMessage(RemdisEmotionMessageDto dto)
        {
            timestamp = dto.Timestamp;
            id = dto.Id;
            producer = dto.Producer;
            update_type = dto.UpdateType;
            exchange = dto.Exchange;
            body = dto.Body;
            data_type = dto.DataType;
        }
    }
}