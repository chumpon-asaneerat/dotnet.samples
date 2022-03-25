#region Using

using System;
using System.Collections.Generic;
using System.Net; // for http status code.
using System.Reflection;

using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace Wpf.Rest.Client
{
    #region IsoDateTimeConverter (Original https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Converters/IsoDateTimeConverter.cs)

    /*
    /// <summary>
    /// Converts a <see cref="DateTime"/> to and from the ISO 8601 date format (e.g. <c>"2008-04-12T12:53Z"</c>).
    /// </summary>
    public class IsoDateTimeConverter : DateTimeConverterBase
    {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private string? _dateTimeFormat;
        private CultureInfo? _culture;

        /// <summary>
        /// Gets or sets the date time styles used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time styles used when converting a date to and from JSON.</value>
        public DateTimeStyles DateTimeStyles
        {
            get => _dateTimeStyles;
            set => _dateTimeStyles = value;
        }

        /// <summary>
        /// Gets or sets the date time format used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time format used when converting a date to and from JSON.</value>
        public string? DateTimeFormat
        {
            get => _dateTimeFormat ?? string.Empty;
            set => _dateTimeFormat = (StringUtils.IsNullOrEmpty(value)) ? null : value;
        }

        /// <summary>
        /// Gets or sets the culture used when converting a date to and from JSON.
        /// </summary>
        /// <value>The culture used when converting a date to and from JSON.</value>
        public CultureInfo Culture
        {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            string text;

            if (value is DateTime dateTime)
            {
                if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                text = dateTime.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
            }
#if HAVE_DATE_TIME_OFFSET
            else if (value is DateTimeOffset dateTimeOffset)
            {
                if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTimeOffset = dateTimeOffset.ToUniversalTime();
                }

                text = dateTimeOffset.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
            }
#endif
            else
            {
                throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)!));
            }

            writer.WriteValue(text);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            bool nullable = ReflectionUtils.IsNullableType(objectType);
            if (reader.TokenType == JsonToken.Null)
            {
                if (!nullable)
                {
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
                }

                return null;
            }

#if HAVE_DATE_TIME_OFFSET
            Type t = (nullable)
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;
#endif

            if (reader.TokenType == JsonToken.Date)
            {
#if HAVE_DATE_TIME_OFFSET
                if (t == typeof(DateTimeOffset))
                {
                    return (reader.Value is DateTimeOffset) ? reader.Value : new DateTimeOffset((DateTime)reader.Value!);
                }

                // converter is expected to return a DateTime
                if (reader.Value is DateTimeOffset offset)
                {
                    return offset.DateTime;
                }
#endif

                return reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }

            string? dateText = reader.Value?.ToString();

            if (StringUtils.IsNullOrEmpty(dateText) && nullable)
            {
                return null;
            }

#if HAVE_DATE_TIME_OFFSET
            if (t == typeof(DateTimeOffset))
            {
                if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
                {
                    return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
                }
                else
                {
                    return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
                }
            }
#endif

            if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
            {
                return DateTime.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
            }
            else
            {
                return DateTime.Parse(dateText, Culture, _dateTimeStyles);
            }
        }
    }
    */

    #endregion

    #region CorrectedIsoDateTimeConverter

    /// <summary>
    /// The CorrectedIsoDateTimeConverter class.
    /// </summary>
    public class CorrectedIsoDateTimeConverter : IsoDateTimeConverter
    {
        #region Consts

        //private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        //private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffK";
        //private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK";
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";

        #endregion

        #region Override methods

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;

                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    if (DateTimeStyles.HasFlag(DateTimeStyles.AssumeUniversal))
                    {
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                    else if (DateTimeStyles.HasFlag(DateTimeStyles.AssumeLocal))
                    {
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                    }
                    else
                    {
                        // Force local
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                    }
                }

                if (DateTimeStyles.HasFlag(DateTimeStyles.AdjustToUniversal))
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                string format = string.IsNullOrEmpty(DateTimeFormat) ? DefaultDateTimeFormat : DateTimeFormat;
                string str = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);
                writer.WriteValue(str);
                //writer.WriteValue(dateTime.ToString(format, Culture));
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }

        #endregion
    }

    #endregion

    #region NJson

    public class NJson
    {
        private static JsonSerializerSettings _defaultSettings = null;
        private static CorrectedIsoDateTimeConverter _dateConverter = new CorrectedIsoDateTimeConverter();
        /// <summary>
        /// Gets Default JsonSerializerSettings.
        /// </summary>
        public static JsonSerializerSettings DefaultSettings
        {
            get
            {
                if (null == _defaultSettings)
                {
                    lock (typeof(NJson))
                    {
                        _defaultSettings = new JsonSerializerSettings()
                        {
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                            DateParseHandling = DateParseHandling.DateTimeOffset,
                            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
                            //DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
                            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
                            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK"
                        };
                        if (null == _defaultSettings.Converters)
                        {
                            // Create new List of not exists.
                            _defaultSettings.Converters = new List<JsonConverter>();
                        }
                        if (null != _defaultSettings.Converters &&
                            !_defaultSettings.Converters.Contains(_dateConverter))
                        {
                            _defaultSettings.Converters.Add(_dateConverter);
                        }
                    }
                }
                return _defaultSettings;
            }
        }
    }

    #endregion

    public class NRestClient
    {
        public static object Get(string baseUrl, string apiUrl,
            int timeout = 1000)
        {
            return Get(baseUrl, apiUrl, string.Empty, string.Empty, timeout);
        }
        public static object Get(string baseUrl, string apiUrl, 
            string username = "", string password = "", int timeout = 1000) 
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
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    client.Authenticator = new HttpBasicAuthenticator(username, password);
                }
                //client.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);
                client.UseNewtonsoftJson(NJson.DefaultSettings);

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
