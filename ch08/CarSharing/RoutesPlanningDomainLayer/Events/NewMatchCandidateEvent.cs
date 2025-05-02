using DDD.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Events
{
    public record NewMatchCandidateEvent<T>(T MatchCandidate) :
        IEventNotification;
    
}
