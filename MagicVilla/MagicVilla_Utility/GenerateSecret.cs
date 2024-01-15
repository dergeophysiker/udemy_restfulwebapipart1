using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;


namespace MagicVilla_Utility
{
    public static class GenerateSecret
    {

           public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        }

        public static void HashPassword()
        {
           // return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }




        public static bool ValidatePassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }


    }
}
