using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver.Entities
{
    internal class RouteRequest: IRouteRequestState
    {
        public Guid Id { get; set; }
        public TownBasicInfo Source { get; set; }=null!;
        public TownBasicInfo Destination { get; set; } = null!;
        public DateTime WhenStart { get; set; }
        public DateTime WhenEnd { get; set; }
        public long TimeStamp { get; set; }

        public UserBasicInfo User { get; set; } = null!;
        public Guid? RouteId { get; set; }
        public RouteOffer? Route { get; set; }
        
    }
}
