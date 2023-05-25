using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.DeliveryComponent
{
    internal class UpsDelivery : DeliveryService
    {
        public UpsDelivery() { }

        public string cancel_deliver(string transactionId)
        {
            throw new NotImplementedException();
        }

        public bool deliver(string address, double weight)
        {
            if (weight > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string deliver(string name, string address, string city, string country, string zip)
        {
            throw new NotImplementedException();
        }
    }
}
