using Antlr.Runtime.Misc;
using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    public interface PaymentService
    {

        /// <returns>transaction_id</returns>
        public string pay(string card_number, string month, string year, string holder, string ccv,string  id);


        /// <returns>Output: '1' if the cancelation has been successful or '-1' if the cancelation has failed.</returns>
        public string cancel_pay(string transaction_id);
    }
}
