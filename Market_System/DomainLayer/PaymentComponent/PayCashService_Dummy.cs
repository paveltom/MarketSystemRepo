using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market_System.DomainLayer.PaymentComponent
{
    class PayCashService_Dummy : PaymentService
    {

        // bayan changed to be singleton
        private static PayCashService_Dummy instance;
        public PayCashService_Dummy()
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

        public string cancel_pay(string transaction_id)
        {
            return "1";
        }



        /// <returns>1</returns>
        public string pay(string card_number, string month, string year, string holder, string ccv, string id)
        {
            return "1";
        }
    }
}