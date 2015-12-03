namespace EaiGuy.ServiceControl.Monitor
{
    using NServiceBus.Logging;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    class UrlQueryHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UrlQueryHelper));

        /// <summary>
        /// Returns JSON string
        /// http://stackoverflow.com/questions/8270464/best-way-to-call-a-json-webservice-from-a-net-console
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                if (errorResponse != null)
                {
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        String errorText = reader.ReadToEnd();
                        log.ErrorFormat("Failed to read URL '{0}'. Error: {1}", url, errorText);
                    }
                }
                else log.ErrorFormat("Failed to read URL '{0}'. Error: {1}", url, ex.ToString());
                throw;
            }
        }
    }
}
