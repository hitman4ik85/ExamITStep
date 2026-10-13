using Azure;
using System;

namespace QueueExam.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // унікальний ідентифікатор
        public string Content { get; set; }                         // текст повідомлення
        public string Receivers { get; set; }                      // імена через кому
        public string ReadBy { get; set; } = "";                   // хто вже прочитав
        public DateTime Timestamp { get; set; } = DateTime.Now;    // дата створення
    }
}
