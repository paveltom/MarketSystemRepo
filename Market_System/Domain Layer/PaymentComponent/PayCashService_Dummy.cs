using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.PaymentComponent
{
    class PayCashService_Dummy : PaymentService
    {
        // bayan changed to be singleton
        private static PayCashService_Dummy instance;
        private PayCashService_Dummy()
        {
        }

        public static PayCashService_Dummy get_instance()
        {
            if (instance == null)
            {
                instance = new PayCashService_Dummy();
            }
            return instance;
        }


        public bool pay(string credit_card, double amount)
        {
            if (amount > 0)
            {
                return true;
            }
            else
                throw new Exception("cannot pay invaild price or credit card details");
        }
    }
}