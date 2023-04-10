using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    internal class PaymentProxy : PaymentService
    {

        public PaymentService paymentService;

        public PaymentService PaymentService
        {
            get { return paymentService; }
            set { paymentService = value; }
        }

        public PaymentProxy(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }

        public Boolean pay(string credit_card, double amount)
        {
            return PaymentService.pay(credit_card,amount);
        }
    }
}