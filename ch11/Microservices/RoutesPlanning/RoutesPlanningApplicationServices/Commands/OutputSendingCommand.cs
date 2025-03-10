using DDD.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.Commands
{
    public class OutputSendingCommand<T>(Func<T, Task> sender, int batchCount, TimeSpan requeueDelay) : ICommand
    {
        public Func<T, Task> Sender => sender;
        public int BatchCount => batchCount;
        public TimeSpan RequeueDelay => requeueDelay;
        public bool OutPutEmpty { get; set; } = false;
    }
}
