using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.DeliveryComponent
{
    internal class DeliveryProxy : DeliveryService
    {

        private static DeliveryService instance;

        public DeliveryService DeliveryService
        {
            get { return instance; }
            set { instance = value; }
        }
        private DeliveryProxy(DeliveryService deliveryService)
        {
        }

        public static DeliveryService get_instance()
        {
            if (instance == null)
            {
                instance = new DeliveryProxy(new HTTPDeliveryService());
            }
            return instance;
        }

        public string deliver(string name, string address, string city, string country, string zip)
        {
            return DeliveryService.deliver(name, address, city, country, zip);
        }

        public string cancel_deliver(string transactionId)
        {
            return DeliveryService.cancel_deliver(transactionId);
        }



        /*
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
        */
 

      
    }
}
