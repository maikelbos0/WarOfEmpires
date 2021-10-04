namespace WarOfEmpires.Utilities.Configuration {
    public sealed class AppSettings {
        public const string Key = "AppSettings";

        public string DatabaseConnectionString { get; set; }
        public string ApplicationBaseUrl { get; set; }
        public string SendGridApiKey { get; set; }
        public string EmailFromAddress { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string BlobStorageContainer { get; set; }
        public string UserImageBaseUrl { get; set; }
        public bool UseCdn { get; set; }
        public string CdnBaseUrl { get; set; }
    }
}
