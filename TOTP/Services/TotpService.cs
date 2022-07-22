using System.Text;
using System.Security.Cryptography;

namespace TOTP.Services
{
    public class TotpService
    {
        protected String secret; // shared secret
        protected readonly int totpLength = 6; // TOTP length
        protected readonly int timeframe = 30; // TOTP maximum validity in seconds

        public TotpService(String secret)
        {
            this.secret = secret;
        }

        /*
         * 
         * TOPT algorithm was taken from
         * 
         * TOTP: Time-Based One-Time Password Algorithm
         * source: https://datatracker.ietf.org/doc/html/rfc6238
         * 
         */

        public String GenerateTotp(String username, int unixtimestamp)
        {
            int counter = GenerateEventCounter(unixtimestamp);

            String input = GenerateUniqueUserInput(username, counter);

            String userSecret = GenerateUserSecret(username);

            long totpHmac = GenerateTotpHMAC(input, userSecret);
            String totpString = ConvertTotpLongToString(totpHmac, this.totpLength);

            return totpString;
        }
        public int GenerateExpiryTime(int unixtimestamp)
        {
            return (unixtimestamp / 30) * 30 + 30;
        }

        protected internal int GenerateEventCounter(int unixtimestamp)
        {
            return unixtimestamp / this.timeframe;
        }

        protected internal String GenerateUniqueUserInput(String username, int counter)
        {
            return counter.ToString() + username;
        }

        protected internal String GenerateUserSecret(String username)
        {
            byte[] hmacComputedHash = GenerateHMAC(username, this.secret);

            return CovertHmacBytesToString(hmacComputedHash);
        }

        protected internal static long GenerateTotpHMAC(String input, String userSecret)
        {
            byte[] hmacComputedHash = GenerateHMAC(input, userSecret);

            return ConvertHmacBytesToLong(hmacComputedHash);
        }

        protected internal static byte[] GenerateHMAC(String input, String secret)
        {
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(secret);
            HMAC hmac = new HMACSHA512(secretkeyBytes);

            byte[] hmacComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

            return hmacComputedHash;
        }

        protected internal static long ConvertHmacBytesToLong(byte[] hmacComputedHash)
        {
            int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
            long result = (hmacComputedHash[offset] & 0x7f) << 24
                | (hmacComputedHash[offset + 1] & 0xff) << 16
                | (hmacComputedHash[offset + 2] & 0xff) << 8
                | (hmacComputedHash[offset + 3] & 0xff) % 1000000;

            return result;
        }

        protected internal static String ConvertTotpLongToString(long input, int digitCount)
        {
            var truncatedValue = ((int)input % (int)Math.Pow(10, digitCount));
            return truncatedValue.ToString().PadLeft(digitCount, '0');
        }

        protected internal static String CovertHmacBytesToString(byte[] hmacComputedHash)
        {
            var sBuilder = new StringBuilder();
            for (int i = 0; i < hmacComputedHash.Length; i++)
            {
                sBuilder.Append(hmacComputedHash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
