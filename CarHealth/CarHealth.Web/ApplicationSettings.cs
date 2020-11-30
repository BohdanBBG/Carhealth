using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Web
{
    public class ApplicationSettings
    {
        public CorsSettings Cors { get; set; }
        public UrlsSettings Urls { get; set; }
        public AuthSettings Auth { get; set; }
    }

    public class UrlsSettings
    {
        public string Api { get; set; }
    }

    public class AuthSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }

    public class CorsSettings
    {
        public CorsSettings()
        {
            AllowedOrigins = new List<string>();
        }

        public List<string> AllowedOrigins { get; set; }
    }

}
