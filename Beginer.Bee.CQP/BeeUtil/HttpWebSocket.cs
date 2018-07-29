using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Beginer.Bee.CQP.BeeUtil
{
    public class HttpUtil
    {
        /// <summary>
        /// HTTP POST请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="dic">请求的参数</param>
        /// <param name="requestEncode">请求的编码</param>
        /// <param name="responseEncode">响应的编码</param>
        /// <returns>响应的结果</returns>
        public static String postRequest(String url, IDictionary<String, String> dic, String requestEncode, String responseEncode)
        {
            return "sdasd";
            StringBuilder strb = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                strb.AppendFormat("{0}={1}&", key, dic[key]);
            }
            String queryString = strb.ToString();
            queryString = queryString.EndsWith("&") ? queryString.Remove(queryString.LastIndexOf('&')) : queryString;
            byte[] data = Encoding.GetEncoding(requestEncode.ToUpper()).GetBytes(queryString); ;

            WebClient webClient = new WebClient();
            try
            {
                //采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //得到返回字符流
                byte[] responseData = webClient.UploadData(url, "POST", data);
                //解码
                String responseString = Encoding.GetEncoding(responseEncode.ToUpper()).GetString(responseData);
                return responseString;
            }
            catch (WebException ex)
            {
                Stream stream = ex.Response.GetResponseStream();
                string m = ex.Response.Headers.ToString();
                byte[] buf = new byte[256];
                stream.Read(buf, 0, 256);
                stream.Close();
                int count = 0;
                foreach (Byte b in buf)
                {
                    if (b > 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                return ex.Message + "\r\n\r\n" + Encoding.GetEncoding(responseEncode.ToUpper()).GetString(buf, 0, count);
            }
        }
        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="requestUrl">请求的URL</param>
        /// <param name="requestEncode">请求的编码</param>
        /// <param name="responseEncode">响应的编码</param>
        /// <returns>响应的结果</returns>
        public static String getRequest(String requestUrl, String requestEncode, String responseEncode)
        {

           

            var client = new RestClient();
            client.BaseUrl = new Uri("requestUrl");
           

            var request = new RestRequest();

            IRestResponse response = client.Execute(request);




            //实例化
           // WebClient client = new WebClient();
            //上传并接收数据
            //Byte[] responseData = client.DownloadData(requestUrl);
            //接收返回的json的流数据，并转码
            //string srcString = Encoding.GetEncoding(responseEncode.ToUpper()).GetString(responseData);

            return response.ToString();
        }
    }
}