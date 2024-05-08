using CGLibs._2FA.OTP;

namespace CGLibs._2FA.HOTP
{
    /// <summary>
    /// 
    /// </summary>
    public class HOTPGenerator : OTPGenerator
    {
        public HOTPGenerator(string secret, int seed = 0, Algorithms algorithm = defaultAlgorithm, short digits = defaultDigits) : base(secret, algorithm, digits)
        {
            _Counter = seed; //set seed so outside application can track counter
        }

        /// <summary>
        /// Generates one time code
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            return base.Generate(Counter);
        }

        private int _Counter = 0;

        private int Counter
        {
            get
            {
                return ++_Counter;
            }
        }

        /// <summary>
        /// The last counter value used to generate OTP
        /// </summary>
        public int LastCounter
        {
            get
            {
                return _Counter;
            }
        }


    }
}
