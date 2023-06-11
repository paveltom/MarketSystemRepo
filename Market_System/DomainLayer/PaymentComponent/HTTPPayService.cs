using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Market_System.DomainLayer.PaymentComponent
{
    public class HTTPPayService : PaymentService
    {

        private static readonly HttpClient client = new HttpClient();
        //private static string url = "https://php-server-try.000webhostapp.com/";
        private static string url;

        public HTTPPayService(string URL) { url = URL; }

        
        public  string pay(string card_number, string month, string year, string holder, string ccv, string id)
        {
            try
            {
                var handshakeContent = new Dictionary<string, string>
                {
                    {"action_type","handshake"},
                };
                string answer = PostRequest(handshakeContent);
                if (!answer.Equals("OK")) { throw new Exception("handshake with payment web service failed"); }
            
                var payContent = new Dictionary<string, string>
                {
                    {"action_type","pay"},
                    {"card_number",card_number},
                    {"month",month },
                    {"year",year },
                    {"holder",holder},
                    { "ccv",ccv },
                    {"id",id },
                };
                string transactionId = PostRequest(payContent);
                if (transactionId.Equals("-1") || transactionId.Equals("unexpected-output")) { throw new Exception("pay request denied by the service"); }
                return transactionId;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string cancel_pay(string transaction_id)
        {
            try
            {
                var handshakeContent = new Dictionary<string, string>
                {
                    {"action_type","handshake"},
                };
                string answer = PostRequest(handshakeContent);
                if (!answer.Equals("OK")) { throw new Exception("handshake with payment web service failed"); }

                var cancel_payContent = new Dictionary<string, string>
                {
                    {"action_type","cancel_pay"},
                    {"transaction_id",transaction_id},
                 
                };
                string result = PostRequest(cancel_payContent);
                if (result.Equals("-1")) { throw new Exception("pay request denied by the service"); }
                return result; //(result = 1 in case of success)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// sending an http request to 'url' web adress.
        /// the content of the post message must contain an action type
        ///The action types supported by the external systems are: handshake, pay,cancel_pay.
        /// </summary>
        /// <returns> the web answer. </returns>
        private string PostRequest(Dictionary<string, string> postContent)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(postContent); //wrap the request as post content
                    var response = client.PostAsync(url, content); //sending the post wo the web service
                    string responseContent = response.Result.Content.ReadAsStringAsync().Result; //getting answer from the web service
                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

 
