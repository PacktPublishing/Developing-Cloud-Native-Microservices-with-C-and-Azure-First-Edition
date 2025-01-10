using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.BasicTypes
{
    public record TimeInterval
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }   
    }
}
