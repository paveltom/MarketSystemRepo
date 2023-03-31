using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.PaymentComponent
{
    internal class PayCashService_Dummy : PaymentService
    {

        public PayCashService_Dummy() { }

        public bool pay(double amount)
        {
            if (amount > 0)
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