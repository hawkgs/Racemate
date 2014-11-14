namespace Racemate.Common
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class QueryStringBuilder
    {
        private const int QUERY_SALT_LEN = 6;

        public static string EncryptRaceId(int id, string name)
        {
            string querySalt = CalculateMD5Hash(name).Substring(0, QUERY_SALT_LEN).ToLower();

            return String.Concat(querySalt, id);
        }

        public static string DecryptRaceId(string queryId)
        {
            if (queryId == null)
            {
                return null;
            }

            string extractedId = queryId.Substring(QUERY_SALT_LEN, queryId.Length - QUERY_SALT_LEN);

            return extractedId;
        }

        // Source: http://blogs.msdn.com/b/csharpfaq/archive/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string_3f00_.aspx
        private static string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                output.Append(hash[i].ToString("X2"));
            }

            return output.ToString();
        }
    }
}
