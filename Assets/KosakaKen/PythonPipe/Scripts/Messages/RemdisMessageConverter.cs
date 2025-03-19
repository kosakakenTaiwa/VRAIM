using System.Text;
using RabbitMQ.Client.Events;
using UnityEngine;

namespace KosakaKen.PythonPipe.Scripts.Messages
{
    /// <summary>
    /// Remdisから送られてくるJSONデータを変換するクラス
    /// </summary>
    public static class RemdisMessageConverter
    {
        public static RemdisMessageDto ToRemdisMessage(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var remdisMessage = JsonUtility.FromJson<RemdisMessage>(Encoding.UTF8.GetString(body));
            return new RemdisMessageDto(remdisMessage);
        }

        public static RemdisEmotionMessageDto ToRemdisEmotionMessage(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var emotionMessage = JsonUtility.FromJson<RemdisEmotionMessage>(Encoding.UTF8.GetString(body));
            return new RemdisEmotionMessageDto(emotionMessage);
        }
    }
}