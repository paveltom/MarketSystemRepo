using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.PaymentComponent
{
    internal class PaymentProxy : PaymentService
    {

        public PaymentService paymentService;

        public PaymentService PaymentService
        {
            get { return paymentService; }
            set { paymentService = value; }
        }
        public Boolean pay(double amount)
        {
            return PaymentService.pay(amount);
        }
    }
}