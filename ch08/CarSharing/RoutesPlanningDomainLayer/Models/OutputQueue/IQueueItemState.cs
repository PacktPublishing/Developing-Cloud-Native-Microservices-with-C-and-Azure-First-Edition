using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.OutputQueue
{
    public  interface IQueueItemState
    {
        Guid Id { get; }
        int MessageCode { get; }
        public string MessageContent { get; }
    }
}
