using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.DeliveryComponent
{
    internal interface DeliveryService
    {
        public Boolean deliver(String address, double weight);
    }
}
