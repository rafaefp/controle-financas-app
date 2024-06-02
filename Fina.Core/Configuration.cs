﻿namespace Fina.Core
{
    public static class Configuration
    {
        public const int DefaultStatusCode = 200;
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 25;
       
        public static string BackendUrl { get; set; } = "http://localhost:5219";
        public static string FrontendUrl { get; set; } = "http://localhost:5141";
    }
}
