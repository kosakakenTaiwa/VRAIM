namespace KosakaKen.PythonPipe.Scripts.Messages
{
    /// <summary>
    /// DTOパターンを適用するためのクラス。
    /// DTO(Data Transfer Object) パターン
    /// </summary>
    [System.Serializable]
    public class RemdisMessageDto
    {
        /// <summary>
        /// メッセージが送られた時間。UNIX時間で表現される。
        /// </summary>
        public readonly long timestamp;
        
        /// <summary>
        /// メッセージに割り当てられた一意のID。
        /// 主にRevokeで特定のデータを削除するときに使う。
        /// </summary>
        public readonly string id;
        
        /// <summary>
        /// メッセージの送信者。
        /// </summary>
        public readonly string producer;
        
        /// <summary>
        /// Remdisで使われるメッセージのタイプ的なもの
        /// </summary>
        public readonly string update_type;
        
        /// <summary>
        /// 送信するルート名
        /// </summary>
        public readonly  string exchange;
        
        /// <summary>
        /// メッセージの中身。
        /// </summary>
        public readonly string body;

        /// <summary>
        /// 送信されたデータの型
        /// </summary>
        public readonly string data_type;

        public RemdisMessageDto
        (
            long timestamp,
            string id,
            string producer,
            string update_type,
            string exchange,
            string body,
            string dataType
        )
        {
            this.timestamp = timestamp;
            this.id = id;
            this.producer = producer;
            this.update_type = update_type;
            this.exchange = exchange;
            this.body = body;
        }

        public RemdisMessageDto(RemdisMessage remdisMessage)
        {
            timestamp = remdisMessage.timestamp;
            id = remdisMessage.id;
            producer = remdisMessage.producer;
            update_type = remdisMessage.update_type;
            exchange = remdisMessage.exchange;
            body = remdisMessage.body;
            data_type = remdisMessage.data_type;
        }
    }
}