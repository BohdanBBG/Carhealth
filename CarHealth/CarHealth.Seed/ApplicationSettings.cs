﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Seed
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
        public string CarHealthIdentityDb { get; set; }
        public string CarsDb { get; set; }
    }

    public class UrlsSettings
    {
        public string Api { get; set; }
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