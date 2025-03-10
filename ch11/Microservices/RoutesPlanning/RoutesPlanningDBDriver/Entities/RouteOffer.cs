using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver.Entities
{
    internal class RouteOffer: IRouteOfferState
    {
        public Guid Id { get; set; }
        public LineString Path { get; set; } = null!;
        public DateTime When { get; set; }
        public UserBasicInfo User { get; set; } = null!;
        public RouteStatus Status { get; set; }
        public ICollection<RouteRequest> Requests { get; set; } = null!;
        public long TimeStamp { get; set; }
    }
}
