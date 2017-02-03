namespace UrlShortnerNetCore
{
    public static class Base58Encoder
    {
        private static string alphabet = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789abcefghijklmnopqrstuvwxyz";
        private static int baseNumber = alphabet.Length;

        public static string Encode(long number)
        {
            var encoded = string.Empty;
            while(number > 0)
            {
                var rem = (int)(number % baseNumber);
                number /= baseNumber;
                encoded = alphabet[rem] + encoded;
            }
            return encoded;
        }
    }
}
