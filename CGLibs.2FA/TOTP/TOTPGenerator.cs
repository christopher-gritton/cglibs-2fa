using CGLibs._2FA.OTP;
using System;
using System.Collections.Generic;

namespace CGLibs._2FA.TOTP
{
    public class TOTPGenerator :OTPGenerator
    {
        public TOTPGenerator(string secret, Algorithms algorithm = defaultAlgorithm, short digits = defaultDigits, int period = defaultPeriod): base(secret, algorithm, digits)
        {
            if (period < 0 || period > 300) throw new Exception("period must be between 0 - 300 and date cannot be null");

            this.Period = period;
        }

        private int Period { get; set; }


        protected const int defaultPeriod = 30;
        private long UnixTimeStamp(DateTime source)
        {
            long epochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
            long unixTime = ((source.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            return unixTime;
        }

        public string GenerateForDate(DateTime? date = null)
        {
            if (date == null) date = DateTime.UtcNow;

            int counter = (int)(UnixTimeStamp(date.Value) / this.Period);

            return Generate(counter);
        }

        public IEnumerable<string> GetValidTotps(TimeSpan timeTolerance)
        {
            var codes = new List<string>();
            var iterationCounter = (int)(UnixTimeStamp(DateTime.UtcNow) / this.Period);
            var iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
            }

            var iterationStart = iterationCounter - iterationOffset;
            var iterationEnd = iterationCounter + iterationOffset;

            for (var counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(Generate(counter));
            }

            return codes.ToArray();
        }


    }
}
