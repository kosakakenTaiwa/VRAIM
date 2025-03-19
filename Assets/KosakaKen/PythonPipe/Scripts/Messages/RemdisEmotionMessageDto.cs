using System;

namespace KosakaKen.PythonPipe.Scripts.Messages
{
    [Serializable]
    public class RemdisEmotionMessageDto
    {
        /// <summary>
        /// メッセージが送られた時間。UNIX時間で表現される。
        /// </summary>
        public readonly long Timestamp;
        
        /// <summary>
        /// メッセージに割り当てられた一意のID。
        /// 主にRevokeで特定のデータを削除するときに使う。
        /// </summary>
        public readonly string Id;
        
        /// <summary>
        /// メッセージの送信者。
        /// </summary>
        public readonly string Producer;
        
        /// <summary>
        /// Remdisで使われるメッセージのタイプ的なもの
        /// </summary>
        public readonly string UpdateType;
        
        /// <summary>
        /// 送信するルート名
        /// </summary>
        public readonly string Exchange;
        
        /// <summary>
        /// メッセージの中身。
        /// </summary>
        public readonly EmotionBody Body;

        public ExpressionType ExpressionType => Body.ExpressionType;
        public ActionType ActionType => Body.ActionType;

        /// <summary>
        /// 送信されたデータの型
        /// </summary>
        public readonly string DataType;

        public RemdisEmotionMessageDto
        (
            long timestamp,
            string id,
            string producer,
            string update_type,
            string exchange,
            EmotionBody body,
            string dataType
        )
        {
            this.Timestamp = timestamp;
            this.Id = id;
            this.Producer = producer;
            this.UpdateType = update_type;
            this.Exchange = exchange;
            this.Body = body;
        }

        public RemdisEmotionMessageDto(RemdisEmotionMessage emotionMessage)
        {
            Timestamp = emotionMessage.timestamp;
            Id = emotionMessage.id;
            Producer = emotionMessage.producer;
            UpdateType = emotionMessage.update_type;
            Exchange = emotionMessage.exchange;
            Body = emotionMessage.body;
            DataType = emotionMessage.data_type;
        }
    }
}