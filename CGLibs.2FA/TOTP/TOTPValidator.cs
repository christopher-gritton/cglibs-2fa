using System;
using System.Linq;

namespace CGLibs._2FA.TOTP
{
    public class TOTPValidator : TOTPGenerator
    {
        public TOTPValidator(string secret, Algorithms algorithm = defaultAlgorithm, short digits = defaultDigits, int period = defaultPeriod) : base(secret, algorithm, digits, period)
        {

        }

        public bool Validate(string clientTotp, int timeToleranceInSeconds = 60)
        {
            var codes = this.GetValidTotps(TimeSpan.FromSeconds(timeToleranceInSeconds));
            return codes.Any(c => c == clientTotp);
        }
    }
}
