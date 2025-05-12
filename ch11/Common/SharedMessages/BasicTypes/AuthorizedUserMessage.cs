using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.BasicTypes
{
    public class AuthorizedUserMessage
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
