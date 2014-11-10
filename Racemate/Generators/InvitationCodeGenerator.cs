namespace Generators
{
    using System;
    using System.Text;

    public static class InvitationCodeGenerator
    {
        private const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int CODE_LEN = 10;

        private static Random generator = new Random();

        public static string Generate()
        {
            StringBuilder code = new StringBuilder();

            for (int i = 0; i < CODE_LEN; i++)
            {
                int charIdx = generator.Next(0, ALLOWED_CHARS.Length);

                code.Append(ALLOWED_CHARS[charIdx]);
            }

            return code.ToString();
        }
    }
}
