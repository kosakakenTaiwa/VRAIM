using System;
using KosakaKen.PythonPipe.Scripts.Messages;
using KosakaKen.Utility.Scripts;
using UnityEngine;

namespace KosakaKen.PythonPipe.Scripts.RemdisLog
{
    /// <summary>
    /// Remdisの送信情報をログに残すためのクラス
    /// </summary>
    public static class RemdisLogger
    {
        public static void Log(RemdisMessageDto dto, RemdisLogOption option = null)
        {
            if (dto.update_type != RemdisUpdateType.ADD) return;
            // ??= について https://qiita.com/tat_tt/items/763d1eee32c22fe67e12
            option ??= new RemdisLogOption();
            
            var log = "";
            
            if (option.IsLogTimeStamp)
            {
                // 日本時間に合わせるためにUTC+9時間の時差を加える
                var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds(dto.timestamp);
                var jstTimestamp = utcTimestamp.ToOffset(new TimeSpan(9, 0, 0));
                log += $"JST TimeStamp : {jstTimestamp} \n";
            }
            if (option.IsLogId) log += $"Id : {dto.id} \n";
            if (option.IsLogProducer) log += $"Producer : {dto.producer} \n";
            if (option.IsLogUpdateType) log += $"Update Type : {dto.update_type} \n";
            if (option.IsLogBody) log += $"Body : {dto.body} \n";
            if (option.IsLogDataType && dto.data_type != string.Empty) log += $"Data Type : {dto.data_type} \n"; 
            if (log.Length > 0) DebugLogger.LogEditorOnly($"{log}");
        }
        
        public static void Log(RemdisEmotionMessageDto dto, RemdisLogOption option = null)
        {
            if (dto.UpdateType != RemdisUpdateType.ADD) return;
            // ??= について https://qiita.com/tat_tt/items/763d1eee32c22fe67e12
            option ??= new RemdisLogOption();
            
            var log = "";
            
            if (option.IsLogTimeStamp)
            {
                // 日本時間に合わせるためにUTC+9時間の時差を加える
                var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds(dto.Timestamp);
                var jstTimestamp = utcTimestamp.ToOffset(new TimeSpan(9, 0, 0));
                log += $"JST TimeStamp : {jstTimestamp} \n";
            }
            if (option.IsLogId) log += $"Id : {dto.Id} \n";
            if (option.IsLogProducer) log += $"Producer : {dto.Producer} \n";
            if (option.IsLogUpdateType) log += $"Update Type : {dto.UpdateType} \n";
            if (option.IsLogBody) log += $"Expression : {dto.Body.ExpressionType} \n";
            if (option.IsLogBody) log += $"Action : {dto.Body.ActionType} \n";
            if (option.IsLogDataType && dto.DataType != string.Empty) log += $"Data Type : {dto.DataType} \n"; 
            if (log.Length > 0) DebugLogger.LogEditorOnly($"{log}");
        }
    }
}