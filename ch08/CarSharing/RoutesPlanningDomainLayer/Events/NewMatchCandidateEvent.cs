using DDD.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Events
{
    public class NewMatchCandidateEvent<T>(T matchCandidate):
        IEventNotification
    {
        public T MatchCandidate => matchCandidate;
    }
}
