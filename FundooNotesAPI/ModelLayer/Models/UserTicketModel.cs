using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Models
{
    public class UserTicketModel
    {
        public string FirstName {  get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }
        public string Token { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
