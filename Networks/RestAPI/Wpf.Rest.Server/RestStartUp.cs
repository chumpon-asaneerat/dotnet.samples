#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// Owin SelfHost
using Owin;
using Microsoft.Owin; // for OwinStartup attribute.
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using System.Web.Http.Validation;
// Owin Authentication
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
// Swagger
using System.Web.Http.Description;
using System.Web.Http.Filters;

//using Swashbuckle.Swagger;
//using Swashbuckle.Application;

#endregion

namespace Wpf.Rest.Server
{
    #region BasicAuthenticationMiddleware

    /// <summary>
    /// The Basic Authentication Owin Middleware.
    /// </summary>
    internal class BasicAuthenticationMiddleware : OwinMiddleware
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The OwinMiddleware instance.</param>
        public BasicAuthenticationMiddleware(OwinMiddleware next) : base(next) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">The OwinContext.</param>
        /// <returns>Returns Task instance.</returns>
        public async override Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;
                if (resp.StatusCode == 401)
                {
                    resp.Headers.Add("WWW-Authenticate", new string[] { "Basic" });
                }
            }, response);

            var header = request.Headers.Get("Authorization");
            if (!string.IsNullOrWhiteSpace(header))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                request.User = null; // set request user

                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase) &&
                    null != Validator)
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (Validator(userName, password))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, userName) };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await Next.Invoke(context);
        }

        #endregion

        #region Static Propertiess

        /// <summary>
        /// Gets or sets validator function.
        /// </summary>
        public static Func<string, string, bool> Validator { get; set; }

        #endregion
    }

    #endregion

    #region CustomBodyModelValidator (has fix code)

    /// <summary>
    /// The Custom Body Model Validator class.
    /// </summary>
    internal class CustomBodyModelValidator : DefaultBodyModelValidator
    {
        /// <summary>
        /// Should Validate Type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <returns>Returns true if specificed need to validate.</returns>
        public override bool ShouldValidateType(Type type)
        {
            //bool isNotDMTModel = !type.IsSubclassOf(typeof(Models.DMTModelBase));
            //TODO: FIXED CODE
            bool isNotDMTModel = true; // hard fix code

            // Ignore validation on all DMTModelBase subclasses.
            return isNotDMTModel && base.ShouldValidateType(type);
        }
    }

    #endregion

    #region AddAuthorizationHeaderParameterOperationFilter
    /*
    /// <summary>
    /// AddAuthorizationHeaderParameterOperationFilter class for swagger.
    /// </summary>
    internal class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            var isAuthorized = filterPipeline
                .Select(filterInfo => filterInfo.Instance)
                .Any(filter => filter is IAuthorizationFilter);

            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                // in some case operation.parameters is null so create new list.
                if (null == operation.parameters) operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "access token",
                    required = true,
                    type = "string"
                });
            }
        }
    }
    */
    #endregion

    public abstract class RestStartUp
    {
    }
}
