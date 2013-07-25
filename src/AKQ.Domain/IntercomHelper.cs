using System.Security.Cryptography;
using System.Text;

namespace AKQ.Domain
{
    public static class IntercomHelper
    {
        public static string HMACSHA256HashString(string stringToHash)
        {
            string intercomApiSecret = "DuUsqEK5OTrqQxWtsCjFfnuau0_aOpph79tNbYbz";
            byte[] secretKey = Encoding.UTF8.GetBytes(intercomApiSecret);
            byte[] bytes = Encoding.UTF8.GetBytes(stringToHash);
            using (var hmac = new HMACSHA256(secretKey))
            {
                hmac.ComputeHash(bytes);
                byte[] data = hmac.Hash;

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}