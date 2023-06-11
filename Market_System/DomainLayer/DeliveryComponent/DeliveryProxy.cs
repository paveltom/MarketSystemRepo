using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.DeliveryComponent
{
    public class DeliveryProxy : DeliveryService
    {

        private static DeliveryService instance;

   
        private DeliveryProxy()
        {
          
        }

        public static DeliveryService get_instance()
        {
            if (instance == null)
            {
                instance = new UpsDelivery();
            }
            return instance;
        }

        public static DeliveryService get_instance(string URL)
        {
            if (instance == null)
            {
                instance = new HTTPDeliveryService(URL);
            }
            return instance;
        }


        public string deliver(string name, string address, string city, string country, string zip)
        {
            return instance.deliver(name, address, city, country, zip);
        }

        public string cancel_deliver(string transactionId)
        {
            return instance.cancel_deliver(transactionId);
        }
    }
}
