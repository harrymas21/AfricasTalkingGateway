using System;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

    public class AfricasTalkingGatewayException : Exception
    {
        public AfricasTalkingGatewayException(string message)
                : base(message)
        { }
        public AfricasTalkingGatewayException(Exception ex) : base(ex.Message, ex)
        {

        }
    }
    public class AfricasTalkingGateway
    {
        private string _username;
        private string _apiKey;
        private string _environment;
        private int responseCode;
        private JavaScriptSerializer serializer;

        //Change the debug flag to true to view the full response
        private Boolean DEBUG = false;

        public AfricasTalkingGateway(string username_, string apiKey_)
        {
            _username = username_;
            _apiKey = apiKey_;
            _environment = "production";
            serializer = new JavaScriptSerializer();
        }

        public AfricasTalkingGateway(string username, string apiKey, string environment)
        {
            _username = username;
            _apiKey = apiKey;
            _environment = environment;
            serializer = new JavaScriptSerializer();
        }

        public object sendMessage(string to_, string message_, string from_ = null, int bulkSMSMode_ = 1, Hashtable options_ = null)
        {
            Hashtable data = new Hashtable();
            data["username"] = _username;
            data["to"] = to_;
            data["message"] = message_;

            if (from_ != null)
            {
                data["from"] = from_;
                data["bulkSMSMode"] = Convert.ToString(bulkSMSMode_);

                if (options_ != null)
                {
                    if (options_.Contains("keyword"))
                    {
                        data["keyword"] = options_["keyword"];
                    }

                    if (options_.Contains("linkId"))
                    {
                        data["linkId"] = options_["linkId"];
                    }

                    if (options_.Contains("enqueue"))
                    {
                        data["enqueue"] = options_["enqueue"];
                    }

                    if (options_.Contains("retryDurationInHours"))
                        data["retryDurationInHours"] = options_["retryDurationInHours"];
                }
            }

            string response = sendPostRequest(data, SMS_URLString);
        if (responseCode == (int)HttpStatusCode.Created)
        {
            var json = serializer.Deserialize<object>(response);
            var rec = JObject.Parse((string)json);
            var recipients = rec["SMSMessageData"]["Recipients"];
            if (recipients.ToString().Length > 0)
            {
                return recipients;
            }
            //  throw new AfricasTalkingGatewayException(rec["SMSMessageData"]["Message"]);
        }
            throw new AfricasTalkingGatewayException(response);
        }

        //SEND POST
        private string sendPostRequest(Hashtable dataMap_, string urlString_)
        {
            try
            {
                string dataStr = "";
                foreach (string key in dataMap_.Keys)
                {
                    if (dataStr.Length > 0) dataStr += "&";
                    string value = (string)dataMap_[key];
                    dataStr += HttpUtility.UrlEncode(key, Encoding.UTF8);
                    dataStr += "=" + HttpUtility.UrlEncode(value, Encoding.UTF8);
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(dataStr);

                System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlString_);

                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Accept = "application/json";

                webRequest.Headers.Add("apiKey", _apiKey);

                Stream webpageStream = webRequest.GetRequestStream();
                webpageStream.Write(byteArray, 0, byteArray.Length);
                webpageStream.Close();

                HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();
                responseCode = (int)httpResponse.StatusCode;
                StreamReader webpageReader = new StreamReader(httpResponse.GetResponseStream());
                string response = webpageReader.ReadToEnd();

                if (DEBUG)
                    Console.WriteLine("Full response: " + response);

                return response;

            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw new AfricasTalkingGatewayException(ex.Message);
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string response = reader.ReadToEnd();

                    if (DEBUG)
                        Console.WriteLine("Full response: " + response);

                    return response;
                }
            }

            catch (AfricasTalkingGatewayException ex)
            {
                throw ex;
            }
        }
        //////////////////////////////////////////////////////////////////////////////
        private bool RemoteCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }

        private string ApiHost
        {
            get
            {
                return (string.ReferenceEquals(_environment, "sandbox") ? "https://api.sandbox.africastalking.com" : "https://api.africastalking.com");
            }
        }

        private string SMS_URLString
        {
            get
            {
                return ApiHost + "/version1/messaging";
            }
        }
    }
