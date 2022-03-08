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
    public abstract class RestStartUp
    {
    }
}
