using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.PaymentProxy
{
    public class PayCashService_dummy : PaymentInterface
    {

        public PayCashService_dummy() { }

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