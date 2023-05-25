﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;

namespace Market_System.DomainLayer.DeliveryComponent
{
    public class HTTPDeliveryService : DeliveryService
    {

        private static readonly HttpClient client = new HttpClient();
        private static string url = "https://php-server-try.000webhostapp.com/";

        public HTTPDeliveryService() { }


        public string deliver(string name, string address, string city, string country, string zip)
        {
            try
            {
                var handshakeContent = new Dictionary<string, string>
                {
                    {"action_type","handshake"},
                };
                string answer = PostRequest(handshakeContent);
                if (!answer.Equals("OK")) { throw new Exception("handshake with delivery web service failed"); }

                var deliveryContent = new Dictionary<string, string>
                {
                    {"action_type","supply"},
                    {"name",name},
                    {"address",address},
                    {"city",city },
                    {"country",country},
                    {"zip",zip },            
                };
                string transactionId = PostRequest(deliveryContent);
                if (transactionId.Equals("-1")) { throw new Exception("deliver request denied by the service"); }
                return transactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string cancel_deliver(string transaction_id)
        {
            try
            {
                var handshakeContent = new Dictionary<string, string>
                {
                    {"action_type","handshake"},
                };
                string answer = PostRequest(handshakeContent);
                if (!answer.Equals("OK")) { throw new Exception("handshake with delivery web service failed"); }

                var cancel_deliveryContent = new Dictionary<string, string>
                {
                    {"action_type","cancel_supply"},
                    {"transaction_id",transaction_id},
                };
                string transactionId = PostRequest(cancel_deliveryContent);
                if (transactionId.Equals("-1")) { throw new Exception("deliver request denied by the service"); }
                return transactionId;//(result = 1 in case of success)
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