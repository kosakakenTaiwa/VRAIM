namespace KosakaKen.PythonPipe.Scripts.RemdisLog
{
    /// <summary>
    /// 会話のログの出力内容を指定するオプション
    /// </summary>
    public class RemdisLogOption
    {
        /// <summary>
        /// タイムスタンプを出力するかどうか
        /// </summary>
        public readonly bool IsLogTimeStamp;
        
        /// <summary>
        /// Idを出力するかどうか
        /// </summary>
        public readonly bool IsLogId;
        
        /// <summary>
        /// Producerを出力するかどうか
        /// </summary>
        public readonly bool IsLogProducer;
        
        /// <summary>
        /// UpdateType(add, commitなど)を出力するかどうか
        /// </summary>
        public readonly bool IsLogUpdateType;
        
        /// <summary>
        /// メッセージを表示するかどうか
        /// </summary>
        public readonly bool IsLogBody;

        /// <summary>
        /// data_typeを出力するか
        /// </summary>
        public readonly bool IsLogDataType;

        public RemdisLogOption
        (
            bool isLogTimeStamp = true,
            bool isLogId = true,
            bool isLogProducer = true,
            bool isLogUpdateType = true,
            bool isLogBody = true,
            bool isLogDataType = true
        )
        {
            IsLogTimeStamp = isLogTimeStamp;
            IsLogId = isLogId;
            IsLogProducer = isLogProducer;
            IsLogUpdateType = isLogUpdateType;
            IsLogBody = isLogBody;
            IsLogDataType = isLogDataType;
        }
    }
}