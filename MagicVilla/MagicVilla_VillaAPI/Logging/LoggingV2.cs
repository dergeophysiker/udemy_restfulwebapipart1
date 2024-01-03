namespace MagicVilla_VillaAPI.Logging
{
    public class LoggingV2 : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("v2 Error - " + message);
            }
            else
            {
                Console.WriteLine("v2" + message);
            }



        }
    }
}
