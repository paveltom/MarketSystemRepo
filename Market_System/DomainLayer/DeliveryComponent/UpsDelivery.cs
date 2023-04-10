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
    }
}
