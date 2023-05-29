using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.DeliveryComponent
{
    public class UpsDelivery : DeliveryService
    {
        public UpsDelivery() { }

        public string cancel_deliver(string transactionId)
        {
            return "1";
        }

        
        public string deliver(string name, string address, string city, string country, string zip)
        {
            return "1";
        }
    }
}
