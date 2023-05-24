using System;
using System.Collections.Generic;
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
                instance = new PaymentProxy(new PayCashService_Dummy());
            }
            return instance;
        }

        public Boolean pay(string credit_card, double amount)
        {
            return PaymentService.pay(credit_card,amount);
        }
    }
}