using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.RouteNegotiation
{
    public class RouteRequestMessage : TimedMessage
    {
        public Guid Id { get; set; }
        public TownBasicInfoMessage? Source { get; set; }
        public TownBasicInfoMessage? Destination { get; set; }
        public TimeIntervalMessage? When { get; set; }
        public UserBasicInfoMessage? User { get; set; }
    }
}
