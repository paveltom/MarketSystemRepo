using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.DeliveryComponent
{
    internal class DeliveryProxy : DeliveryService
    {

        private static DeliveryProxy instance;
        private DeliveryProxy()
        {
        }

        public static DeliveryProxy get_instance()
        {
            if (instance == null)
            {
                instance = new DeliveryProxy();
            }
            return instance;
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
        public bool deliver(string address, double weight)
        {
            if (weight > 0)
            {
                return true;
            }

            throw new Exception("cannot deliver due to address or weight");
            
        }
    }
}
