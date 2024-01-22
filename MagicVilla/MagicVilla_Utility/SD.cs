namespace MagicVilla_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,POST, PUT, DELETE
        }

        public static string SessionTokenKeyName = "JWTToken";
        public static string TargetApiVersion = "v1";
    
    }
}