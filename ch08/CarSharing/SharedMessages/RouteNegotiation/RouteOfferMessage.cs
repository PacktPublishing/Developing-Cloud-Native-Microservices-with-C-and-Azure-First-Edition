using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.RouteNegotiation
{
    public class RouteOfferMessage : TimedMessage
    {
        public Guid Id { get; set; }
        public IList<TownBasicInfoMessage>? Path { get; set; }
        public DateTime? When { get; set; }
        public UserBasicInfoMessage? User { get; set; }
    }
}
