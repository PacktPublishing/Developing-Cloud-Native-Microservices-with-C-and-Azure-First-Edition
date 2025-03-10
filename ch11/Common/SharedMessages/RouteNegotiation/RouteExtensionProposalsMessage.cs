using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.RouteNegotiation
{
    public class RouteExtensionProposalsMessage : TimedMessage
    {
        public Guid RouteId { get; set; }
        public IList<RouteRequestMessage>? Proposals { get; set; }
    }
}
