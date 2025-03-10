using RoutesPlanningDomainLayer.Models.OutputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver.Entities
{
    internal class OutputQueueItem: IQueueItemState
    {
        public Guid Id { get; set; }
        public int MessageCode { get; set; }
        public string MessageContent { get; set; } = null!;
        public DateTime ReadyTime { get; set; }
    }
}
