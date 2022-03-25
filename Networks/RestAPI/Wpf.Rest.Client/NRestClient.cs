#region Using

using System;
using System.Net; // for http status code.
using System.Reflection;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace Wpf.Rest.Client
{
    public class NRestClient
    {
        public static object Get(string baseUrl, string apiUrl, int timeout = 1000) 
        {
            object ret = null;
            string actionUrl = (!apiUrl.StartsWith("/")) ? @"/" + apiUrl : apiUrl;
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                var client = new RestClient(baseUrl);
                // Note:
                // timeout used when connect is establish but server is not response
                // but in case server is not avaliable or cannot reach server due to network failed
                // the client, response is return around 4s (that ignore timeout value).
                client.Timeout = timeout;
                //client.ReadWriteTimeout = int.MaxValue;

                var request = new RestRequest(actionUrl, Method.GET);
                //request.Timeout = client.Timeout;
                //request.ReadWriteTimeout = client.ReadWriteTimeout;
                var response = client.Execute(request);
                if (null != response)
                {
                    if (!response.IsSuccessful)
                    {
                        Console.WriteLine("response failed.");

                        var ex = response.ErrorException;
                        if (null != ex)
                        {
                            if (response.ErrorException is WebException)
                            {
                                var status = (ex as WebException).Status;
                                Console.WriteLine(status);
                            }
                            else Console.WriteLine(ex.GetType());
                        }

                        switch (response.ResponseStatus)
                        {
                            case ResponseStatus.Aborted:
                                break;
                            case ResponseStatus.Completed:
                                break;
                            case ResponseStatus.Error:
                                break;
                            case ResponseStatus.TimedOut:
                                break;
                            default: // ResponseStatus.None:
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("response success.");
                    }

                    var respStatus = response.ResponseStatus;
                    int httpStatusCode = (int)response.StatusCode;
                    string desc = response.StatusDescription;

                    Console.WriteLine("response is not null. response status: {0}, http status code {1}, http status desc: {2}",
                        respStatus, httpStatusCode, desc);
                    Console.WriteLine("error: {0}", response.ErrorMessage);

                    //HttpStatus status = (code >= 200 && code <= 399) ? HttpStatus.Success : HttpStatus.Failed;
                    if (
                        (httpStatusCode >= 200 && httpStatusCode <= 399) && // test code
                        // response.IsSuccessful() && 
                        null != response.Content)
                    {
                        ret = response.Content;
                        Console.WriteLine("P1");
                    }
                    else
                    {
                        ret = null;
                        Console.WriteLine("P2");
                    }
                }
                else
                {
                    // response object is null
                    Console.WriteLine("Response object is null.");
                }
            }
            catch (Exception ex)
            {
                // unknown error
                Console.WriteLine("Unknown error: Exception: {0}", ex);
            }
            return ret;
        }
    }
}
