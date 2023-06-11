using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    public class PaymentProxy : PaymentService
    {

        private static PaymentService instance;


    
        private PaymentProxy()
        {
            
        }


        public static PaymentService get_instance()
        {
            if (instance == null)
            {
                instance = new PayCashService_Dummy();
            }
            return instance;
        }

        public static PaymentService get_instance(string URL)
        {
            if (instance == null)
            {
                instance = new HTTPPayService(URL);
            }
            return instance;
        }



        public string pay(string card_number, string month, string year, string holder, string ccv, string id)
        {
            return instance.pay(card_number, month, year, holder, ccv, id);
        }

        public string cancel_pay(string transaction_id)
        {
            return instance.cancel_pay(transaction_id);
        }
    }
}