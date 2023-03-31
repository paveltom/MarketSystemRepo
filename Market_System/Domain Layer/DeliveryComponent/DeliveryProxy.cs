using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.DeliveryComponent
{
    internal class DeliveryProxy : DeliveryService
    {
        private DeliveryService deliveryService;

        public DeliveryService DeliveryService
        {
            get { return deliveryService; }
            set { deliveryService = value; }
        }

        public DeliveryProxy(DeliveryService deliveryService)
        {
            this.DeliveryService = deliveryService;
        }

        public bool deliver(string address, double weight)
        {
            return DeliveryService.deliver(address, weight);
        }
    }
}
