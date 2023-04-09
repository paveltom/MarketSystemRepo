using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    internal interface PaymentService
    {
        public Boolean pay(string credit_card,double amount);
    }
}
