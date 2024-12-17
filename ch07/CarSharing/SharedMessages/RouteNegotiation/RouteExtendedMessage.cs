using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.RouteNegotiation
{
    public class RouteExtendedMessage : TimedMessage
    {
        public RouteOfferMessage? ExtendedRoute {  get; set; }
        public IList<RouteRequestMessage>? AddedRequests { get; set; }
        public bool Closed { get; set; }
    }
}
