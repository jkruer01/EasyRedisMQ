using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Services
{
    public interface INotificationService
    {
        Task NotifyOfNewMessagesAsync(object message);
    }
}
