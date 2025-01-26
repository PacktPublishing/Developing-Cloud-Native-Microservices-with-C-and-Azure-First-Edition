using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.BasicTypes
{
    public class TownBasicInfoMessage
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public GeoLocalizationMessage? Location { get; set; }
    }
}
