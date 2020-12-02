using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4
{
    public class ApplicationSettings
    {
        public CorsSettings Cors { get; set; }
        public MongoDbSettings MongoDb { get; set; }
        public UrlsSettings Urls { get; set; }

        public EFCoreDbSettings EFCoreDb { get; set; }
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string MainDb { get; set; }
        public string MongoDbIdentity { get; set; }
    }

    public class EFCoreDbSettings
    {
        public string ClientsIdentityDb { get; set; }
        public string UsersIdentityDb { get; set; }
        public string CarsDb { get; set; }
    }

    public class UrlsSettings
    {
        public string Api { get; set; }
        public string Identity { get; set; }
        public string WebSpa { get; set; }
        public string WebSpaReact { get; set; }
        
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
