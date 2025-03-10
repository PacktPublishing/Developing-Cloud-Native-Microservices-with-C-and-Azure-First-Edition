using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.BasicTypes
{
    public record UserBasicInfo()
    {
        public Guid Id { get; init; }
        public string DisplayName { get; init; } = null!;
    }
}
