namespace ProjectPlutus.Extensions.Models
{
    public class LoggingEvents
    {
        public const int GetItem = 1000;
        public const int PostItem = 1001;
        public const int PatchItem = 1002;
        public const int Diagnostics = 2000;
        public const int Validates = 3000;
        public const int ItemNotFound = 4000;
        public const int ServerError = 5000;
    }
}
