using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.Route
{
    public interface IRouteOfferState
    {
        Guid Id { get; }
        LineString Path { get; set; }
        DateTime When { get; }
        UserBasicInfo User { get; }
        RouteStatus Status { get; set; }
        public long TimeStamp { get; set; }
    }
}
