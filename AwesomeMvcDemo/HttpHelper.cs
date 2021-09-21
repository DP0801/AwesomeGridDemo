using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AwesomeMvcDemo
{
    public class HttpHelper
    {
        public static HttpResponse SendHTTPRequest(string url, string method, string contentType, string data)
        {
            var response = new HttpResponse();

            try
            {
                var wrGETURL = WebRequest.Create(url);

                wrGETURL.Method = method;

                if (!string.IsNullOrEmpty(contentType))
                    wrGETURL.ContentType = contentType;

                using (var stream = new StreamWriter(wrGETURL.GetRequestStream()))
                {
                    stream.Write(data);
                }

                var webresponse = wrGETURL.GetResponse() as HttpWebResponse;

                var enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                var loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                response.RawResponse = loResponseStream.ReadToEnd();
                response.StatusCode = webresponse.StatusCode;
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessage = ex.ToString();
            }

            return response;
        }
    }
}