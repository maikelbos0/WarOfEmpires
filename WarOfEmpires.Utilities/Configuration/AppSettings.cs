namespace WarOfEmpires.Utilities.Configuration {
    public sealed class AppSettings {
        public string DatabaseConnectionString { get; set; }
        public string ApplicationBaseUrl { get; set; }
        public string SendGridApiKey { get; set; }
        public string EmailFromAddress { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string BlobStorageContainer { get; set; }
    }
}
