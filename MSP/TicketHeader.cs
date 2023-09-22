using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPCreator.MSP
{
    internal struct TicketHeader
    {
        public string Ticket { get; set; }
        public object anyAttribute { get; set; }
    }
}
