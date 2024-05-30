namespace Fina.Api
{
    public static class ApiConfiguration
    {
        public const string UserId = "fina@fina.com";

        public static string ConnectionString { get; set; } = string.Empty;
        public static string CorsPolicyName { get; set; } = "wasm";
    }
}
