using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.BasicTypes
{
    public record TownBasicInfo
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public Point Location { get; init; } = null!;
    }
    
}
