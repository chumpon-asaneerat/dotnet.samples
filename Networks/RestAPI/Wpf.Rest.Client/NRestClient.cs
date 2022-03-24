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
                //client.ReadWriteTimeout = int.MaxValue;
                client.Timeout = timeout; // NOTE. if value over than 4 seconds is always timeout at 4.12 seconds.

                var request = new RestRequest(actionUrl, Method.GET);
                request.ReadWriteTimeout = client.ReadWriteTimeout;
                request.Timeout = client.Timeout;

                var response = client.Execute(request);
                if (null != response)
                {
                    if (!response.IsSuccessful)
                    {
                        Console.WriteLine("response failed. status: {0}", response.ResponseStatus);

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

                    int code = (int)response.StatusCode;
                    string desc = response.StatusDescription;

                    Console.WriteLine("response is not null. code {0}, desc: {1}", code, desc);

                    //HttpStatus status = (code >= 200 && code <= 399) ? HttpStatus.Success : HttpStatus.Failed;
                    if (
                        (code >= 200 && code <= 399) && // test code
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
                    Console.WriteLine("P3");
                }
            }
            catch (Exception)
            {
                // timeout
                Console.WriteLine("P4");
            }
            return ret;
        }
    }
}
