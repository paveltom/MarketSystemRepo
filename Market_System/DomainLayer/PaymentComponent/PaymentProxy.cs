using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    internal class PaymentProxy : PaymentService
    {

        public static PaymentService instance;


        public PaymentService PaymentService
        {
            get { return instance; }
            set { instance = value; }
        }

        public PaymentProxy(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }


        public static PaymentService get_instance()
        {
            if (instance == null)
            {
                instance = new PaymentProxy(new HTTPPayService());
            }
            return instance;
        }

        public string pay(string card_number, string month, string year, string holder, string ccv, string id)
        {
            return PaymentService.pay(card_number, month, year, holder, ccv, id);
        }

        public string cancel_pay(string transaction_id)
        {
            return PaymentService.cancel_pay(transaction_id);
        }
    }
}