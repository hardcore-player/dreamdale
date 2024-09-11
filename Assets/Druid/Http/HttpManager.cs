using System;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System.Text;

namespace Druid
{
    public class HttpManager
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cb"></param>
        public static void HttpGet(string url, Dictionary<string, string> headers, Action<HTTPResponse> cb)
        {
            HTTPRequest request = _HttpRequest(url, headers, HTTPMethods.Get, cb);
            request.Send();
        }


        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>Cache
        /// <param name="postData"></param>
        /// <param name="cb"></param>
        public static void HttpPost(string url, object postdata, Dictionary<string, string> headers, Action<HTTPResponse> cb)
        {
            string data = LitJson.JsonMapper.ToJson(postdata);
            Debug.LogWarning("HttpPost:" + data);

            byte[] rdata = Encoding.UTF8.GetBytes(data);

            HTTPRequest request = _HttpRequest(url, headers, HTTPMethods.Post, cb);
            request.RawData = rdata;
            request.Send();
        }

        //Http相关
        public static HTTPRequest _HttpRequest(string url, Dictionary<string, string> headerCntr, HTTPMethods hTTPMethods, Action<HTTPResponse> cb)
        {
            Debug.Log("_HttpRequest:" + url);

            HTTPRequest request = new HTTPRequest(new Uri(url), hTTPMethods, (req, resp) =>
            {
                try
                {
                    Debug.Log("_HttpRequest req.State: " + req.State);
                    switch (req.State)
                    {

                        // The request finished without any problem.
                        case HTTPRequestStates.Finished:
                            if (resp.IsSuccess)
                            {
                                DateTime downloadStarted = (DateTime)req.Tag;
                                TimeSpan diff = DateTime.Now - downloadStarted;

                                string text = string.Format("Streaming finished in {0:N0}ms", diff.TotalMilliseconds);

                                Debug.LogWarning(text);

                                string text0 = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                               resp.StatusCode,
                                                               resp.Message,
                                                               resp.DataAsText);
                                Debug.LogWarning(text0);
                                Debug.LogWarning(resp.DataAsText);
                            }
                            else
                            {
                                string text = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                                resp.StatusCode,
                                                                resp.Message,
                                                                resp.DataAsText);
                                Debug.LogWarning(text);

                                request = null;
                            }
                            break;

                        // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                        case HTTPRequestStates.Error:
                            string text1 = "Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
                            Debug.LogError(text1);

                            request = null;
                            break;

                        // The request aborted, initiated by the user.
                        case HTTPRequestStates.Aborted:
                            string text2 = "Request Aborted!";
                            Debug.LogWarning(text2);

                            request = null;
                            break;

                        // Connecting to the server is timed out.
                        case HTTPRequestStates.ConnectionTimedOut:
                            string text3 = "Connection Timed Out!";
                            Debug.LogError(text3);

                            request = null;
                            break;

                        // The request didn't finished in the given time.
                        case HTTPRequestStates.TimedOut:
                            string text4 = "Processing the request Timed Out!";
                            Debug.LogError(text4);

                            request = null;
                            break;
                    }

                    cb(resp);

                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("----------------------catch" + ex.Message + "------------------------------");
                }
            });

            if (headerCntr != null)
            {
                foreach (KeyValuePair<string, string> item in headerCntr)
                {
                    request.SetHeader(item.Key, item.Value);
                }
            }

            //request.CustomCertificateVerifyer = new CustomVerifier();
            //request.UseAlternateSSL = true;
            //request.Proxy = null;
            request.Tag = DateTime.Now;
            //request.Send();

            return request;
        }
    }
}

