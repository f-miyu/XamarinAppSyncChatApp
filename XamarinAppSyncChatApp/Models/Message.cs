using System;
namespace XamarinAppSyncChatApp.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ChatId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}
