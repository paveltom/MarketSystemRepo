using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Market_System.DomainLayer.DeliveryComponent
{
    internal interface DeliveryService
    {
        public string deliver(string name, string address, string city, string country, string zip);

        public string cancel_deliver(String transactionId);

    }
}
