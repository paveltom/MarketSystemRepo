using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.PaymentComponent
{
    class PayCashService_Dummy : PaymentService
    {

        private static readonly HttpClient client = new HttpClient();
        private string url = "https://php-server-try.000webhostapp.com/";



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

/*





    public static async Task Main()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Ensure a successful response

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

 * 
 * /