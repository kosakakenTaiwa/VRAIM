namespace KosakaKen.PythonPipe.Scripts.Messages
{
    /// <summary>
    /// Remdisから送られてくるJSONデータを変換するためのクラス
    /// </summary>
    [System.Serializable]
    public class RemdisMessage
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
        public string body;

        /// <summary>
        /// 送信されたデータの型
        /// </summary>
        public string data_type;

        public RemdisMessage(RemdisMessageDto dto)
        {
            timestamp = dto.timestamp;
            id = dto.id;
            producer = dto.producer;
            update_type = dto.update_type;
            exchange = dto.exchange;
            body = dto.body;
            data_type = dto.data_type;
        }
        
        public RemdisMessage(){}
    }
}