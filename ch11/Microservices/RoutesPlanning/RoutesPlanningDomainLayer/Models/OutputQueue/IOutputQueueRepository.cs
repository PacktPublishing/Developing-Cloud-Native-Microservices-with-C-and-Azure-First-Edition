using DDD.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.OutputQueue
{
    public interface IOutputQueueRepository: IRepository
    {
        Task<IList<QueueItem>> Take(int N, TimeSpan requeueAfter);
        void Confirm(Guid[] ids);
        QueueItem New<T>(T item, int messageCode);
    }
}
