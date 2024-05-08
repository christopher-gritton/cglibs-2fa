using System;
using System.Security.Cryptography;

namespace CGLibs._2FA.OTP
{

    /// <summary>
    /// Algorithm based on RFC4226
    /// </summary>
    /// <remarks><![CDATA[https://datatracker.ietf.org/doc/html/rfc4226]]> RFC4226</remarks>
    public class OTPGenerator
    {

        /// <summary>
        /// Create new OTP Generator
        /// </summary>
        public OTPGenerator(string secret, Algorithms algorithm = defaultAlgorithm, short digits = defaultDigits)
        {
            if (digits < 6 || digits > 8 || string.IsNullOrWhiteSpace(secret)) throw new Exception("Iterations limited to 6-8, secret must not be null");

            this.Secret = secret;
            this.Algorithm = algorithm;
            this.Digits = digits;
        }

        private string Secret { get; set; }
        private Algorithms Algorithm { get; set; }
        private short Digits { get; set; }

        protected const short defaultDigits = 6; //default digits - allowed 6-8
        protected const Algorithms defaultAlgorithm = Algorithms.SHA1; //default algorithm - allowed controlled by enumeration

        /// <summary>
        /// 
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        private int DigitModulo(int digit)
        {
            return (int)Math.Pow(10, digit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counter"></param>
        /// <returns></returns>
        public string Generate(long counter)
        {
            
            HMAC _hmac = null;
            byte[] key = System.Text.Encoding.UTF8.GetBytes(this.Secret);

            switch (this.Algorithm)
            {
                case Algorithms.SHA1:
                    _hmac = new HMACSHA1(key);
                    break;
                case Algorithms.SHA256:
                    _hmac = new HMACSHA256(key);
                    break;
                case Algorithms.SHA512:
                    _hmac = new HMACSHA512(key);
                    break;
                case Algorithms.MD5:
                    _hmac = new HMACMD5(key);
                    break;
            }

            byte[] hmacComputedHash = _hmac.ComputeHash(GetBigEndianBytes(counter));

            int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0xf;

            var truncatedhash = (hmacComputedHash[offset] & 0x7f) << 24
                | (hmacComputedHash[offset + 1] & 0xff) << 16
                | (hmacComputedHash[offset + 2] & 0xff) << 8
                | (hmacComputedHash[offset + 3] & 0xff);


            var pinValue = (int)truncatedhash % (int)DigitModulo(this.Digits);

            //clean up hashing
            _hmac.Dispose();

            return pinValue.ToString().PadLeft(this.Digits, '0');
        }

        /// <summary>
        /// converts a long into a big endian byte array.
        /// </summary>
        /// <remarks>
        /// RFC 4226 specifies big endian as the method for converting the counter to data to hash.
        /// </remarks>
        static internal byte[] GetBigEndianBytes(long input)
        {
            // Since .net uses little endian numbers, we need to reverse the byte order to get big endian.
            var data = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            return data;
        }

        /// <summary>
        /// converts an int into a big endian byte array.
        /// </summary>
        /// <remarks>
        /// RFC 4226 specifies big endian as the method for converting the counter to data to hash.
        /// </remarks>
        static internal byte[] GetBigEndianBytes(int input)
        {
            // Since .net uses little endian numbers, we need to reverse the byte order to get big endian.
            var data = BitConverter.GetBytes(input);
            Array.Reverse(data);
            return data;
        }

    }
}
