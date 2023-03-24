using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.WebApplication.Wrappers
{
    public class SessionWrapper
    {
        private static IHttpContextAccessor m_httpContextAccessor;
        public static HttpContext Current => m_httpContextAccessor.HttpContext;
        public static string ReturnUrl
        {
            get
            {
                string returnUrl = Current.Session.GetString("ReturnUrl");
                Current.Session.Remove("ReturnUrl");
                return returnUrl;
            }
            set => Current.Session.SetString("ReturnUrl", value);
        }

        public static void SetReturnUrlToCurrent()
        {
            ReturnUrl = Current.Request.Path;
        }

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }

    }
}
