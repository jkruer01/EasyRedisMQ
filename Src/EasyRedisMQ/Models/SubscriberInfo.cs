using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Models
{
    public class SubscriberInfo
    {
        public string SubscriberId { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string Topic { get; set; }
    }
}
