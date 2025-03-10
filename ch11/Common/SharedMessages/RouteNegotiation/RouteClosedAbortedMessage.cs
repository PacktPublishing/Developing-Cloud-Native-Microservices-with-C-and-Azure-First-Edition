using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.RouteNegotiation
{
    public class RouteClosedAbortedMessage: TimedMessage
    {
        public Guid RouteId { get; set; }
        public bool IsAborted { get; set; }
    }
}
